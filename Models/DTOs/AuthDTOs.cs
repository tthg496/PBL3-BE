namespace ParkingManagement.BLL.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Gender { get; set; } // Male, Female, Other
    }

    public class VerifyOtpDto
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }

    public class LoginResultDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Role { get; set; }       // Customer / Employee / Manager
        public string? AccountId { get; set; }
        public string? FullName { get; set; }
        public string? RelatedId { get; set; }  // CustomerId hoặc EmployeeId
    }

    public class RegisterResultDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Email { get; set; }
        public string? CustomerId { get; set; }
        public string? Otp { get; set; }
    }

    // ── Employee Account Management DTOs ────────────────────
    /// <summary>
    /// Thông tin tài khoản nhân viên
    /// </summary>
    public class EmployeeAccountInfoDto
    {
        public string EmployeeId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;  // Username
        public string PhoneNumber { get; set; } = null!;
        public string? Shift { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    /// <summary>
    /// Đổi mật khẩu result
    /// </summary>
    public class ChangePasswordResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }
}
