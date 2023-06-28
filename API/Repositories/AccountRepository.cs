using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class AccountRepository : GeneralRepository<Account>, IAccountRepository
    {
        public AccountRepository(BookingDbContext context) : base(context) { }

        public Account? CheckOtp(string email, int otp)
        {
            return _context.Set<Account>().Join(_context.Set<Employee>(),
                a => a.Guid, e => e.Guid, (a, e) =>
                new { Account = a, Employee = e }).FirstOrDefault(e => e.Employee.Email == email && e.Account.Otp == otp)?.Account;

            /*var employee = _context.Set<Employee>().FirstOrDefault(e => e.Email == email);

            if (employee == null)
            {
                return null;
            }

            return _context.Set<Account>().FirstOrDefault(a => a.Guid == employee.Guid && a.Otp == otp);*/

        }
    }
}
