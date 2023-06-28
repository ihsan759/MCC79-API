using API.Models;

namespace API.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        Employee? GetByEmailAndPhoneNumber(string data);
    }
}
