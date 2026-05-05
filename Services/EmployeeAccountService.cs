using System.Text.RegularExpressions;
using BCrypt.Net;
using ParkingManagement.BLL.DTOs;
using ParkingManagement.BLL.Services.Interfaces;
using ParkingManagement.DAL.Interfaces;

namespace ParkingManagement.BLL.Services.Implementations
{
    /// <summary>
    /// Employee Account Management
    /// </summary>
    public class EmployeeAccountService : IEmployeeAccountService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAccountRepository _accountRepository;

        public EmployeeAccountService(
            IEmployeeRepository employeeRepository,
            IAccountRepository accountRepository)
        {
            _employeeRepository = employeeRepository;
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Lấy thông tin tài khoản của nhân viên
        /// </summary>
        public async Task<EmployeeAccountInfoDto> GetAccountInfoAsync(string employeeId)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(employeeId);
                if (employee == null)
                    throw new Exception("Nhân viên không tồn tại");

                var account = employee.Account;
                if (account == null)
                    throw new Exception("Tài khoản không tồn tại");

                return new EmployeeAccountInfoDto
                {
                    EmployeeId = employee.EmployeeId,
                    FullName = employee.FullName,
                    Email = account.Username,
                    PhoneNumber = employee.PhoneNumber ?? "",
                    Shift = employee.Shift,
                    IsActive = account.IsActive,
                    CreatedAt = account.CreatedAt,
                    LastLoginAt = null  // Account không có LastLoginAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin tài khoản: {ex.Message}");
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        public async Task<ChangePasswordResultDto> ChangePasswordAsync(string employeeId, ChangePasswordDto request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                    return new ChangePasswordResultDto
                    {
                        Success = false,
                        Message = "Mật khẩu hiện tại không được để trống"
                    };

                // Validate new password
                var passwordValidation = ValidatePassword(request.NewPassword);
                if (!passwordValidation.IsValid)
                    return new ChangePasswordResultDto
                    {
                        Success = false,
                        Message = passwordValidation.ErrorMessage
                    };

                // Check: NewPassword = ConfirmPassword
                if (request.NewPassword != request.ConfirmPassword)
                    return new ChangePasswordResultDto
                    {
                        Success = false,
                        Message = "Mật khẩu xác nhận không khớp"
                    };

                // Check: NewPassword != CurrentPassword
                if (request.NewPassword == request.CurrentPassword)
                    return new ChangePasswordResultDto
                    {
                        Success = false,
                        Message = "Mật khẩu mới không được giống mật khẩu cũ"
                    };

                // Tìm employee
                var employee = await _employeeRepository.GetByIdAsync(employeeId);
                if (employee == null)
                    return new ChangePasswordResultDto
                    {
                        Success = false,
                        Message = "Nhân viên không tồn tại"
                    };

                var account = employee.Account;
                if (account == null)
                    return new ChangePasswordResultDto
                    {
                        Success = false,
                        Message = "Tài khoản không tồn tại"
                    };

                // Verify current password
                if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, account.PasswordHash))
                    return new ChangePasswordResultDto
                    {
                        Success = false,
                        Message = "Mật khẩu hiện tại không chính xác"
                    };

                // Hash new password
                var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

                // Update password
                account.PasswordHash = newPasswordHash;
                await _accountRepository.UpdateAsync(account);

                return new ChangePasswordResultDto
                {
                    Success = true,
                    Message = "Đổi mật khẩu thành công"
                };
            }
            catch (Exception ex)
            {
                return new ChangePasswordResultDto
                {
                    Success = false,
                    Message = $"Lỗi đổi mật khẩu: {ex.Message}"
                };
            }
        }

        // ── VALIDATION HELPER ──────────────────────────────────

        /// <summary>
        /// Validate Password theo copilot-instructions.md:
        /// Min 6 chars, có letters + numbers + special chars
        /// </summary>
        private (bool IsValid, string ErrorMessage) ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Mật khẩu không được để trống");

            if (password.Length < 6)
                return (false, "Mật khẩu phải ít nhất 6 ký tự");

            // Check: has letters
            if (!Regex.IsMatch(password, @"[a-zA-Z]"))
                return (false, "Mật khẩu phải chứa chữ cái (a-z hoặc A-Z)");

            // Check: has numbers
            if (!Regex.IsMatch(password, @"[0-9]"))
                return (false, "Mật khẩu phải chứa chữ số (0-9)");

            // Check: has special characters
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':"",.<>?/\\|`~]"))
                return (false, "Mật khẩu phải chứa ký tự đặc biệt (!@#$%^&*...)");

            return (true, "");
        }
    }
}
