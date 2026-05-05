using ParkingManagement.BLL.DTOs;

namespace ParkingManagement.BLL.Services.Interfaces
{
    /// <summary>
    /// Employee Account Management
    /// </summary>
    public interface IEmployeeAccountService
    {
        /// <summary>
        /// Lấy thông tin tài khoản của nhân viên
        /// </summary>
        Task<EmployeeAccountInfoDto> GetAccountInfoAsync(string employeeId);

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        Task<ChangePasswordResultDto> ChangePasswordAsync(string employeeId, ChangePasswordDto request);
    }
}
