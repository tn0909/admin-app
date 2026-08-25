using AdminApp.Auth;
using AdminApp.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto request)
        {
            if (!_authService.ValidateCredentials(request.Username, request.Password))
            {
                return Unauthorized();
            }

            var token = _authService.GenerateToken(request.Username);
            return Ok(new LoginResponseDto { Token = token });
        }
    }
}
