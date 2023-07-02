using API.DTOs.Bookings;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("Api/Bookings")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _service;

        public BookingController(BookingService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetBooking();

            if (entities == null)
            {
                return NotFound(new ResponseHandlers<GetBookingDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandlers<IEnumerable<GetBookingDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("Get-booking-now")]
        public IActionResult GetBookingNow()
        {
            var entities = _service.BookingNow();

            if (entities == null)
            {
                return NotFound(new ResponseHandlers<BookingRoomToday>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandlers<IEnumerable<BookingRoomToday>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("Deails")]
        public IActionResult GetBookingDetails()
        {
            var entities = _service.GetBookingDetails();

            if (entities == null)
            {
                return NotFound(new ResponseHandlers<BookingDetailsDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandlers<IEnumerable<BookingDetailsDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("Deails/{guid}")]
        public IActionResult GetBookingDetails(Guid guid)
        {
            var entities = _service.GetBookingDetailsByGuid(guid);

            if (entities == null)
            {
                return NotFound(new ResponseHandlers<BookingDetailsDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandlers<BookingDetailsDto>
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
            var booking = _service.GetBooking(guid);
            if (booking is null)
            {
                return NotFound(new ResponseHandlers<GetBookingDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandlers<GetBookingDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = booking
            });
        }

        [HttpPost]
        public IActionResult Create(NewBookingDto newBookingDto)
        {
            var createBooking = _service.CreateBooking(newBookingDto);
            if (createBooking is null)
            {
                return BadRequest(new ResponseHandlers<GetBookingDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new ResponseHandlers<GetBookingDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createBooking
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateBookingDto updateBookingDto)
        {
            var update = _service.UpdateBooking(updateBookingDto);
            if (update is -1)
            {
                return NotFound(new ResponseHandlers<UpdateBookingDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (update is 0)
            {
                return BadRequest(new ResponseHandlers<UpdateBookingDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check your data"
                });
            }
            return Ok(new ResponseHandlers<UpdateBookingDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteBooking(guid);

            if (delete is -1)
            {
                return NotFound(new ResponseHandlers<GetBookingDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (delete is 0)
            {
                return BadRequest(new ResponseHandlers<GetBookingDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check connection to database"
                });
            }

            return Ok(new ResponseHandlers<GetBookingDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }
    }
}
