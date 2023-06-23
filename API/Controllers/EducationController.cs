using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Api/Educations")]
    public class EducationController : ControllerBase
    {
        private readonly IEducationRepository _repository;

        public EducationController(IEducationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var education = _repository.GetAll();
            if (!education.Any())
            {
                return NotFound();
            }

            return Ok(education);
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var education = _repository.GetByGuid(guid);
            if (education is null)
            {
                return NotFound();
            }

            return Ok(education);
        }

        [HttpPost]
        public IActionResult Create(Education education)
        {
            var isCreated = _repository.Create(education);
            return Ok(isCreated);
        }

        [HttpPut]
        public IActionResult Update(Education education)
        {
            var isUpdated = _repository.Update(education);
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
