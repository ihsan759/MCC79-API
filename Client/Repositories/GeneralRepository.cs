using API.Utilities;
using Client.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace Client.Repositories
{
    public class GeneralRepository<Entity, TId> : IRepository<Entity, TId>
        where Entity : class
    {
        private readonly string request;
        private readonly HttpClient httpClient;

        public GeneralRepository(string request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7103/api/")
            };
            this.request = request;
        }

        public async Task<ResponseHandlers<Entity>> Delete(TId id)
        {
            ResponseHandlers<Entity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            using (var response = httpClient.DeleteAsync(request + "?guid=" + id).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<Entity>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseHandlers<IEnumerable<Entity>>> Get()
        {
            ResponseHandlers<IEnumerable<Entity>> entityVM = null;
            using (var response = await httpClient.GetAsync(request))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<IEnumerable<Entity>>>(apiResponse);
            }
            return entityVM;
        }

        public Task<ResponseHandlers<Entity>> Get(TId id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseHandlers<Entity>> Post(Entity entity)
        {
            ResponseHandlers<Entity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<Entity>>(apiResponse);
            }
            return entityVM;
        }

        public Task<ResponseHandlers<Entity>> Put(TId id, Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
