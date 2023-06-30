using API.Models;

namespace API.Contracts
{
    public interface IAccountRoleRepository : IGeneralRepository<AccountRole>
    {
        ICollection<AccountRole> GetByGuidEmployee(Guid employeeGuid);
    }
}
