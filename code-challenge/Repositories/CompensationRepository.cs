using challenge.Data;
using challenge.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public CompensationRepository(ILogger<IEmployeeRepository> logger, CompensationContext compContext)
        {
            _compensationContext = compContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _logger.LogInformation("Add CompensationRepository by id: " + compensation.CompensationId + "\t" + "for Employee "+ compensation.EmployeeId);
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }

        public Compensation GetCompensationById(string id)
        {
            _logger.LogInformation("Get Compensation detail by id: " + id);
            return _compensationContext.Compensations.FirstOrDefault(c => c.CompensationId == id);
        }

        public Compensation GetCompensationByEmployeeId(string employee_id)
        {
            _logger.LogInformation("Get Compensation for employee: " + employee_id);
            return _compensationContext.Compensations.OrderByDescending(c => c.EffectiveDate).FirstOrDefault(c => c.EmployeeId == employee_id);
        }
    }
}