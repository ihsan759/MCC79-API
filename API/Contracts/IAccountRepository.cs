using API.Models;

namespace API.Contracts
{
    public interface IAccountRepository : IGeneralRepository<Account>
    {
        Account? CheckOtp(string email, int otp);
    }
}
