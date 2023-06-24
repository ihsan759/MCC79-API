using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        private readonly BookingDbContext _context;

        public EmployeeRepository(BookingDbContext context) : base(context) { }

    }
}
