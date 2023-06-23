using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/universities")]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityRepository _repository;

        public UniversityController(IUniversityRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var univerities = _repository.GetAll();

            if (!univerities.Any())
            {
                return NotFound();
            }

            return Ok(univerities);
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var university = _repository.GetByGuid(guid);
            if (university is null)
            {
                return NotFound();
            }

            return Ok(university);
        }

        [HttpPost]
        public IActionResult Create(University university)
        {
            var isCreated = _repository.Create(university);
            return Ok(isCreated);
        }

        [HttpPut]
        public IActionResult Update(University university)
        {
            var isUpdated = _repository.Update(university);
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
