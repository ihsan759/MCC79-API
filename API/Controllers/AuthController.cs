using API.DTOs.Auth;
using API.DTOs.Bookings;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("Api/Auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public ActionResult Register(RegisterDto registerDto)
        {
            var createRegister = _authService.Register(registerDto);
            if (createRegister is null)
            {
                return BadRequest(new ResponseHandlers<GetBookingDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new ResponseHandlers<RegisterDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createRegister
            });
        }

        [HttpPost("login")]
        public ActionResult Login(LoginDto loginDto)
        {
            var loginResult = _authService.Login(loginDto);
            if (loginResult == "0")
            {
                return NotFound(new ResponseHandlers<LoginDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Account not found"
                });
            }

            if (loginResult == "-1")
            {
                return BadRequest(new ResponseHandlers<LoginDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Password is incorrect"
                });
            }

            if (loginResult == "-2")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandlers<LoginDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving when creating token"
                });
            }

            return Ok(new ResponseHandlers<String>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Login Success",
                Data = loginResult
            });
        }


        [HttpPut("changePassword")]
        public IActionResult Update(ChangePasswordDto changePasswordDto)
        {
            var update = _authService.ChangePassword(changePasswordDto);
            if (update is -1)
            {
                return NotFound(new ResponseHandlers<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Email not Found"
                });
            }
            if (update is 0)
            {
                return NotFound(new ResponseHandlers<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Otp doesn't match"
                });
            }
            if (update is 1)
            {
                return NotFound(new ResponseHandlers<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Otp has been used"
                });
            }
            if (update is 2)
            {
                return NotFound(new ResponseHandlers<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Otp alredy expired"
                });
            }
            return Ok(new ResponseHandlers<ChangePasswordDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }
    }
}
