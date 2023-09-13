using Microsoft.AspNetCore.Mvc;
using SampleEmployeeProject.Services;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    // single Employee creation
    [HttpPost("create")]
    public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { status = "Error", message = "Validation failed." });

        var result = await _employeeService.CreateEmployeeAsync(employee);

        return CreatedAtAction(nameof(CreateEmployee), new { id = result.Id }, result);
    }

    // Mass Employee creation
    [HttpPost("masscreate")]
    public async Task<ActionResult> MassCreateEmployees(List<Employee> employees)
    {
        var results = await _employeeService.CreateEmployeesAsync(employees);
        return CreatedAtAction(nameof(MassCreateEmployees), new { count = results.Count });
    }

    // Mass Employee Excell import
    [HttpPost("excelimport")]
    public async Task<ActionResult<ResponseModel>> ImportEmployeesFromExcel()
    {
        if (Request.Form.Files.Count == 0)
            return BadRequest(new { status = "Error", message = "No files uploaded." });

        var file = Request.Form.Files[0];
        using var stream = file.OpenReadStream();

        var response = await _employeeService.ImportEmployeesFromExcelAsync(stream);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }
}
