using ParkingManagement.BLL.DTOs;

namespace ParkingManagement.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResultDto> LoginAsync(LoginDto dto);
        Task<RegisterResultDto> RegisterAsync(RegisterDto dto);
        Task<RegisterResultDto> VerifyOtpAsync(VerifyOtpDto dto);
        Task<string> SendOtpAsync(string email);
        Task<EmployeeRegistrationResultDto> RegisterEmployeeAsync(EmployeeRegisterDto dto);
    }
}
