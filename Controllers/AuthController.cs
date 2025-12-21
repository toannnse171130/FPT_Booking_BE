using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Services;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            if (string.IsNullOrEmpty(request.Email) ||
                !request.Email.Trim().EndsWith("@fpt.edu.vn", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = "Vui lòng sử dụng email FPT (@fpt.edu.vn) để đăng nhập." });
            }

            var token = await _authService.Login(request);

            if (token == null)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng!" });
            }

            return Ok(new { token = token });
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            Console.WriteLine("Received Google login request with ID token: " + request.IdToken);
            var token = await _authService.LoginWithGoogle(request.IdToken);

            if (token == null)
            {
                return BadRequest(new { message = "Đăng nhập Google thất bại hoặc email không hợp lệ!" });
            }

            return Ok(new { token = token });
        }


    }
}