using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.Authentication;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response { StatusCode = StatusCodes.Status404NotFound, Message = "Invalid user details." });
            }

            // Check If User exists
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return Conflict(new Response { StatusCode = StatusCodes.Status409Conflict, Message = "User already exists!" });
            }

            var user = new IdentityUser()
            {
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errorMessage = "User creation failed! Please check user details and try again.";
                foreach (var error in result.Errors)
                {
                    errorMessage += $" {error.Description}";
                }
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { StatusCode = StatusCodes.Status500InternalServerError, Message = errorMessage });
                
            }
            return Ok(new Response { StatusCode = StatusCodes.Status200OK, Message = "User created successfully!" });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response { StatusCode = StatusCodes.Status404NotFound, Message = "Invalid user credentials." });
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized(new Response { StatusCode = StatusCodes.Status401Unauthorized, Message = "Unauthorized User!" });
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                return BadRequest(new Response { StatusCode = StatusCodes.Status400BadRequest, Message = "Invalid Password!" });
            }

            return Ok(new Response { StatusCode = StatusCodes.Status200OK, Message = "User Logged In!" });
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new Response { StatusCode = StatusCodes.Status200OK, Message = "Logged out successfully!" });
        }
    }
}
