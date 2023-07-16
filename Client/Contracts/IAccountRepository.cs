using API.Utilities;
using Client.DTOs.Auth;
using Client.Repositories;

namespace Client.Contracts
{
    public interface IAccountRepository
    {
        public Task<ResponseHandlers<string>> Login(LoginDto entity);
        public Task<ResponseHandlers<AccountRepository>> Register(RegisterDto entity);
    }
}
