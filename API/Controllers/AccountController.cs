using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repository;

        public AccountController(IAccountRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var account = _repository.GetAll();
            if (!account.Any())
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var account = _repository.GetByGuid(guid);
            if (account is null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public IActionResult Create(Account account)
        {
            var isCreated = _repository.Create(account);
            return Ok(isCreated);
        }

        [HttpPut]
        public IActionResult Update(Account account)
        {
            var isUpdated = _repository.Update(account);
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
