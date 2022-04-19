using challenge.Models;
using System;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Compensation Create(Compensation compensation);
        Compensation GetById(string id);
        Compensation GetByEmployeeId(string employee_id);
    }
}