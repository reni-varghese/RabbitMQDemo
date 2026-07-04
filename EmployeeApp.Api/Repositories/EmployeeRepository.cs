using EmployeeApp.Api.Data;
using EmployeeApp.Api.Models;

namespace EmployeeApp.Api.Repositories
{
    public class EmployeeRepository :Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) :base(context)
        {
            
        }
    }
}
