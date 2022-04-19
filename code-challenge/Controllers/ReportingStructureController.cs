using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;


namespace challenge.Controllers
{
    [Route("api/reportingstructure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }



        [HttpGet("{employeeId}", Name = "getNumberOfReports")]
        public IActionResult GetNumberOfReports(String employeeId)
        {
            _logger.LogDebug($"Received Get Reporting Structure request for Employee '{employeeId}'");

            Employee employee = _employeeService.GetById(employeeId);

            if (employee == null)
                return NotFound();

            ReportingStructure reportingStructure = _employeeService.GetReportingStructure(employee);

            return Ok(reportingStructure);
        }


    }
}