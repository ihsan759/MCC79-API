using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(BookingDbContext context) : base(context) { }

        public Employee? GetByEmailAndPhoneNumber(string data)
        {
            return _context.Set<Employee>().FirstOrDefault(e => e.PhoneNumber == data || e.Email == data);
        }

        public Employee? CheckEmail(string email)
        {
            return _context.Set<Employee>().FirstOrDefault(e => e.Email == email);
        }
    }
}
