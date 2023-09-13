namespace SampleEmployeeProject.Services
{
    public interface IEmployeeService
    {
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<List<Employee>> CreateEmployeesAsync(List<Employee> employees);
        Task<ResponseModel> ImportEmployeesFromExcelAsync(Stream fileStream);
    }
}
