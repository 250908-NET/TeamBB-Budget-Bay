



using Microsoft.AspNetCore.Mvc;
using BudgetBay.Services;
using BudgetBay.DTOs;

namespace BudgetBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = await _authService.Register(registerUserDto);

            if (newUser is null)
            {
                return BadRequest("A user with this email or username already exists.");
            }

            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _authService.Login(loginUserDto);

            if (token == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(token); // maybe create a TokenDto 
        }
    }
}