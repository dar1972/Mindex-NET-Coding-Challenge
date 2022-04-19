using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;


namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<EmployeeController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.EmployeeId} for  {compensation.Salary}'");
            _compensationService.Create(compensation);
            return CreatedAtAction("GetCompensationById", new { id = compensation.CompensationId }, compensation);
        }

        [HttpGet("{employee_id}", Name = "getCompensation")]
        public IActionResult GetCompensation(String employee_id)
        {
            _logger.LogDebug($"Received CompensationController.GetCompensation get request for '{employee_id}'");

            Compensation compensation = _compensationService.GetByEmployeeId(employee_id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }

        [HttpGet("GetCompensationById/{id}")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received compensation get request for employee id '{id}'");

            Compensation compensation = _compensationService.GetById(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }




    }
}