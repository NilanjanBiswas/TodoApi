using Microsoft.AspNetCore.Mvc;
using TodoApi.Common;
using TodoApi.Dtos;
using TodoApi.Service;

namespace TodoApi.Controller
{
    
    public class AuthController : ApiV1BaseController
    {
        private readonly IAuthService _auth;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService auth, ILogger<AuthController> logger) : base(logger)
        {
            _auth = auth;
            _logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupDto dto)
        {
            _logger.LogInformation("Signup request for {Email}", dto.Email);
            var res = await _auth.SignupAsync(dto);
            if (string.IsNullOrEmpty(res.Token))
                return ToActionResult(new ApiResponse<AuthResponseDto>(null, false, "Signup failed", false) { });
            return ToActionResult(new ApiResponse<AuthResponseDto>(res));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            _logger.LogInformation("Login attempt for {Email}", dto.Email);
            var res = await _auth.LoginAsync(dto);
            if (string.IsNullOrEmpty(res.Token))
                return ToActionResult(new ApiResponse<AuthResponseDto>(null, false, "Invalid credentials", false));
            return ToActionResult(new ApiResponse<AuthResponseDto>(res));
        }
    }
}
