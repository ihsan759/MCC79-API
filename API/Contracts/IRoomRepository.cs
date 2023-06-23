using API.Models;

namespace API.Contracts
{
    public interface IRoomRepository
    {
        ICollection<Room> GetAll();
        Room? GetByGuid(Guid guid);
        Room Create(Room university);
        bool Update(Room university);
        bool Delete(Guid guid);
    }
}
