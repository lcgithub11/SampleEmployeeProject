using OfficeOpenXml;
using System.Reflection.Metadata;

namespace SampleEmployeeProject.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly MyDbContext _context;

        public EmployeeService(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a single emmployee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);

            if (employee.FamilyMembers != null && employee.FamilyMembers.Any())
            {
                await _context.FamilyMembers.AddRangeAsync(employee.FamilyMembers);
            }

            await _context.SaveChangesAsync();
            return employee;
        }

        /// <summary>
        /// Crate mass employee from List of  <<Employee>>
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public async Task<List<Employee>> CreateEmployeesAsync(List<Employee> employees)
        {
            await _context.Employees.AddRangeAsync(employees);

            List<FamilyMember> familyMembers = employees
                .Where(e => e.FamilyMembers != null && e.FamilyMembers.Any())
                .SelectMany(e => e.FamilyMembers)
                .ToList();

            if (familyMembers.Any())
            {
                await _context.FamilyMembers.AddRangeAsync(familyMembers);
            }

            await _context.SaveChangesAsync();
            return employees;
        }


        /// <summary>
        /// Let's assuming we have 2 excels for  Employees and Family Members:
        /// This one is sample excel for employees
        //      A            B               C                      D                   E                       F
        //1	RegisterNumber  Name         Surname                Birthdate           Email                   PhoneNumber
        //2	001	            Lokman         Ceylan	            01/01/1991	lokmanceylan1991@gmail.com      05305042991
        //3	002	            Ali            Yılmaz	            02/11/1990	aliyilmaz@gmail.com             05305042991

        /// This one is sample excel for employees
        //  A                     B         C                   D           E            F
        //1	EmployeeRegNo       Name    Surname             Birthdate   PhoneNumber     Type
        //2	001	                Berk    Ceylan	            01/01/2010	1234567891	    Child
        //3	001	                Ayşe    Ceylan	            01/01/2012	1234567892	    Child
        //4	002	                Fatma   Yılmaz	            02/02/1993	0987654320	    Spouse

        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public async Task<ResponseModel> ImportEmployeesFromExcelAsync(Stream fileStream)
        {
            try
            {
                using var package = new ExcelPackage(fileStream);

                // Employees worksheet
                ExcelWorksheet employeeSheet = package.Workbook.Worksheets[0];
                int totalEmployeeRows = employeeSheet.Dimension.Rows;

                //Our approach is to use two worksheets within the same Excel file:
                //One worksheet for employees.
                //One worksheet for family members.
                //Each row in this worksheet corresponds to a family member, and there's a column indicating to which employee the family member belongs like RegisterNumber

                List<Employee> employees = new List<Employee>();

                for (int i = 2; i <= totalEmployeeRows; i++)
                {
                    Employee employee = new Employee
                    {
                        RegisterNumber = employeeSheet.Cells[i, 1].Value.ToString(),
                        Name = employeeSheet.Cells[i, 2].Value.ToString(),
                        Surname = employeeSheet.Cells[i, 3].Value.ToString(),
                        Birthdate = DateTime.Parse(employeeSheet.Cells[i, 4].Value.ToString()),
                        Email = employeeSheet.Cells[i, 5].Value.ToString(),
                        PhoneNumber = employeeSheet.Cells[i, 6].Value.ToString(),
                    };
                    employees.Add(employee);
                }
                await CreateEmployeesAsync(employees);

                // Family members worksheet
                ExcelWorksheet familySheet = package.Workbook.Worksheets[1];
                int totalFamilyRows = familySheet.Dimension.Rows;

                List<FamilyMember> familyMembers = new List<FamilyMember>();

                for (int i = 2; i <= totalFamilyRows; i++)
                {
                    string registerNumber = familySheet.Cells[i, 1].Value.ToString();
                    Employee correspondingEmployee = employees.FirstOrDefault(e => e.RegisterNumber == registerNumber);

                    if (correspondingEmployee == null)
                        throw new Exception($"No employee found for register number {registerNumber} in the family members worksheet.");

                    // We Assuming 
                    FamilyMember familyMember = new FamilyMember
                    {
                        EmployeeId = correspondingEmployee.Id,
                        Name = familySheet.Cells[i, 2].Value.ToString(),
                        Surname = familySheet.Cells[i, 3].Value.ToString(),
                        Birthdate = DateTime.Parse(familySheet.Cells[i, 4].Value.ToString()),
                        PhoneNumber = familySheet.Cells[i, 5].Value.ToString(),
                        Type = Enum.Parse<FamilyType>(familySheet.Cells[i, 6].Value.ToString())
                    };

                    familyMembers.Add(familyMember);
                }

                _context.FamilyMembers.AddRange(familyMembers);
                await _context.SaveChangesAsync();

                return new ResponseModel(true, "Employees and their family members imported successfully!");
            }
            catch (Exception ex)
            {
                return new ResponseModel(false, ex.Message);
            }
        }

    }
}
