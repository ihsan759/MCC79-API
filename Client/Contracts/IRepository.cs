using API.Utilities;

namespace Client.Contracts
{
    public interface IRepository<T, X>
        where T : class
    {
        Task<ResponseHandlers<IEnumerable<T>>> Get();
        Task<ResponseHandlers<T>> Get(X id);
        Task<ResponseHandlers<T>> Post(T entity);
        Task<ResponseHandlers<T>> Put(X id, T entity);
        Task<ResponseHandlers<T>> Delete(X id);
    }
}
