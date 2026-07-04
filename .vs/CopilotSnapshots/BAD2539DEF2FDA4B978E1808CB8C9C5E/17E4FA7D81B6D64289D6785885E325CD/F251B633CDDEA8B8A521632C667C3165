using EmployeeApp.Api.Models.Dtos;

namespace EmployeeApp.Api.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string UserId)> Register(RegisterDto request);
        Task<(bool Success,string Message,string token,int ExpiresIn)> Login(LoginDto request);
       
            
     }
}
