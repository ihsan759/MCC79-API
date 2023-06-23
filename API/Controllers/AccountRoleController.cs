using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Api/AccountRole/roles")]
    public class AccountRoleController : ControllerBase
    {
        private readonly IAccountRoleRepository _repository;

        public AccountRoleController(IAccountRoleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var accountRoles = _repository.GetAll();
            if (!accountRoles.Any())
            {
                return NotFound();
            }

            return Ok(accountRoles);
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var accountRole = _repository.GetByGuid(guid);
            if (accountRole is null)
            {
                return NotFound();
            }

            return Ok(accountRole);
        }

        [HttpPost]
        public IActionResult Create(AccountRole accountRole)
        {
            var isCreated = _repository.Create(accountRole);
            return Ok(isCreated);
        }

        [HttpPut]
        public IActionResult Update(AccountRole accountRole)
        {
            var isUpdated = _repository.Update(accountRole);
            if (!isUpdated)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var isDeleted = _repository.Delete(guid);
            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
