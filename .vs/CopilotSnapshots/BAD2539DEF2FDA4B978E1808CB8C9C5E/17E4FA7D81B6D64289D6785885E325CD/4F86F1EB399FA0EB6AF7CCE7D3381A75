using EmployeeApp.Api.Models;
using EmployeeApp.Api.Models.Dtos;
using EmployeeApp.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {

            var (success,message,userId)=await service.Register(request);

            if (!success)
            {
                return BadRequest(new { message });
            }
            return Ok(new {message,userId});
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDto request)
        {
            var (success, message,token,expiresIn)  = await service.Login(request);
            if (!success)
            {
                return Unauthorized(new { message });
            }

            AuthResponse response = new AuthResponse
            {
                AccessToken=token,
                Message=message,
                ExpiresIn=expiresIn
            };

            return Ok(response);


        }
    }

    
}
