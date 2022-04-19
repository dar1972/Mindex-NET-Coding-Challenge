using challenge.Models;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation Add(Compensation compensation);
        Compensation GetCompensationById(string id);
        Compensation GetCompensationByEmployeeId(string employee_id);
        Task SaveAsync();
    }
}