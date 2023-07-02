using API.DTOs.Universities;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/universities")]
    [Authorize(Roles = "User")]
    public class UniversityController : ControllerBase
    {
        private readonly UniversityService _service;

        public UniversityController(UniversityService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var entities = _service.GetUniversity();

            if (entities == null)
            {
                return NotFound(new ResponseHandlers<GetUniversityDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandlers<IEnumerable<GetUniversityDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var university = _service.GetUniversity(guid);
            if (university is null)
            {
                return NotFound(new ResponseHandlers<GetUniversityDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandlers<GetUniversityDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = university
            });
        }

        [HttpPost]
        public IActionResult Create(NewUniversityDto newUniversityDto)
        {
            var createdUniversity = _service.CreateUniversity(newUniversityDto);
            if (createdUniversity is null)
            {
                return BadRequest(new ResponseHandlers<GetUniversityDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new ResponseHandlers<GetUniversityDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createdUniversity
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateUniversityDto updateUniversityDto)
        {
            var update = _service.UpdateUniversity(updateUniversityDto);
            if (update is -1)
            {
                return NotFound(new ResponseHandlers<UpdateUniversityDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (update is 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandlers<UpdateUniversityDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check your data"
                });
            }
            return Ok(new ResponseHandlers<UpdateUniversityDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteUniversity(guid);

            if (delete is -1)
            {
                return NotFound(new ResponseHandlers<GetUniversityDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (delete is 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandlers<GetUniversityDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check connection to database"
                });
            }

            return Ok(new ResponseHandlers<GetUniversityDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }

        [HttpGet("by-name/{name}")]
        public IActionResult GetByName(string name)
        {
            var universities = _service.GetUniversity(name);
            if (!universities.Any())
            {
                return NotFound(new ResponseHandlers<GetUniversityDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "No universities found with the given name"
                });
            }

            return Ok(new ResponseHandlers<IEnumerable<GetUniversityDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Universities found",
                Data = universities
            });
        }
    }
}
