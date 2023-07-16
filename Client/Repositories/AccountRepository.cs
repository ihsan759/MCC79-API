using API.Utilities;
using Client.Contracts;
using Client.DTOs.Auth;
using Newtonsoft.Json;
using System.Text;

namespace Client.Repositories
{
    public class AccountRepository : GeneralRepository<RegisterDto, Guid>, IAccountRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;
        public AccountRepository(string request = "auth/") : base(request)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7103/api/")
            };
            this.request = request;
        }

        public async Task<ResponseHandlers<string>> Login(LoginDto entity)
        {
            ResponseHandlers<string> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "login", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<string>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseHandlers<AccountRepository>> Register(RegisterDto entity)
        {
            ResponseHandlers<AccountRepository> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "register", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<AccountRepository>>(apiResponse);
            }
            return entityVM;
        }
    }
}
