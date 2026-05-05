using BCrypt.Net;
using ParkingManagement.BLL.DTOs;
using ParkingManagement.BLL.Services.Interfaces;
using ParkingManagement.DAL.Models;
using ParkingManagement.DAL.Interfaces;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace ParkingManagement.BLL.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IEmployeeInviteRepository _inviteRepo;
        private readonly IOtpRepository _otpRepo;
        private static Dictionary<string, RegisterDto> _registerDataStorage = new(); // Lưu trữ dữ liệu đăng ký tạm thời

        public AuthService(
            IAccountRepository accountRepo, 
            ICustomerRepository customerRepo, 
            IEmployeeRepository employeeRepo, 
            IEmployeeInviteRepository inviteRepo,
            IOtpRepository otpRepo)
        {
            _accountRepo = accountRepo;
            _customerRepo = customerRepo;
            _employeeRepo = employeeRepo;
            _inviteRepo = inviteRepo;
            _otpRepo = otpRepo;
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto dto)
        {
            // Tìm tài khoản theo email
            var account = await _accountRepo.GetByEmailAsync(dto.Email.Trim().ToLower());
            if (account == null || !account.IsActive)
                return new LoginResultDto { Success = false, Message = "Email hoặc mật khẩu không đúng." };

            // Verify bcrypt
            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, account.PasswordHash);
            if (!valid)
                return new LoginResultDto { Success = false, Message = "Email hoặc mật khẩu không đúng." };

            // Tìm RelatedId (CustomerId / EmployeeId / ManagerId)
            string? relatedId = null;
            string? fullName = null;

            if (account.Role == "Customer")
            {
                var customer = await _customerRepo.GetByAccountIdAsync(account.AccountId);
                relatedId = customer?.CustomerId;
                fullName = customer?.FullName;
            }
            else if (account.Role == "Employee")
            {
                // TODO: Implement IEmployeeRepository để lấy EmployeeId
                relatedId = account.AccountId;
                fullName = account.Manager?.FullName ?? account.Username;
            }
            else if (account.Role == "Manager")
            {
                // TODO: Implement IManagerRepository để lấy ManagerId
                relatedId = account.AccountId;
                fullName = account.Manager?.FullName ?? account.Username;
            }

            return new LoginResultDto
            {
                Success = true,
                Role = account.Role,
                AccountId = account.AccountId,
                FullName = fullName,
                RelatedId = relatedId
            };
        }

        /// <summary>
        /// Đăng ký - Bước 1: Validate và gửi OTP
        /// </summary>
        public async Task<RegisterResultDto> RegisterAsync(RegisterDto dto)
        {
            // 1. Validate thông tin bắt buộc
            if (string.IsNullOrWhiteSpace(dto.FullName) || string.IsNullOrWhiteSpace(dto.Email) || 
                string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                return new RegisterResultDto 
                { 
                    Success = false, 
                    Message = "Vui lòng nhập đầy đủ các trường bắt buộc." 
                };
            }

            // 2. Kiểm tra email hợp lệ
            if (!IsValidEmail(dto.Email))
            {
                return new RegisterResultDto 
                { 
                    Success = false, 
                    Message = "Địa chỉ email không hợp lệ." 
                };
            }

            // 3. Kiểm tra password và confirm password
            if (dto.Password != dto.ConfirmPassword)
            {
                return new RegisterResultDto 
                { 
                    Success = false, 
                    Message = "Mật khẩu xác nhận không trùng khớp." 
                };
            }

            // 4. Kiểm tra độ mạnh của password
            if (!IsStrongPassword(dto.Password))
            {
                return new RegisterResultDto 
                { 
                    Success = false, 
                    Message = "Mật khẩu phải có ít nhất 6 ký tự, bao gồm chữ cái, chữ số và ký tự đặc biệt. Ví dụ: Huong@4906" 
                };
            }

            // 5. Kiểm tra email đã tồn tại
            var email = dto.Email.Trim().ToLower();
            if (await _accountRepo.ExistsEmailAsync(email))
            {
                return new RegisterResultDto 
                { 
                    Success = false, 
                    Message = "Email này đã được đăng ký." 
                };
            }

            // 6. Gửi OTP
            var otpCode = await SendOtpAsync(email);

            // 7. Lưu dữ liệu đăng ký tạm thời (sẽ xóa sau khi xác thực OTP)
            _registerDataStorage[email] = dto;

            return new RegisterResultDto 
            { 
                Success = true, 
                Message = "OTP xác thực đã được gửi đến email của bạn.", 
                Email = email,
                Otp = otpCode
            };
        }

        /// <summary>
        /// Xác thực OTP - Bước 2: Tạo tài khoản nếu OTP hợp lệ
        /// </summary>
        public async Task<RegisterResultDto> VerifyOtpAsync(VerifyOtpDto dto)
        {
            var email = dto.Email.Trim().ToLower();

            // Kiểm tra OTP hợp lệ
            if (!await _otpRepo.IsValidOtpAsync(email, dto.Otp.Trim()))
            {
                return new RegisterResultDto 
                { 
                    Success = false, 
                    Message = "OTP không hợp lệ hoặc đã hết hạn." 
                };
            }

            // Kiểm tra dữ liệu đăng ký
            if (!_registerDataStorage.ContainsKey(email))
            {
                return new RegisterResultDto 
                { 
                    Success = false, 
                    Message = "Dữ liệu đăng ký không tồn tại. Vui lòng đăng ký lại." 
                };
            }

            var registerData = _registerDataStorage[email];

            // Tạo Account
            var accountId = $"ACC{DateTime.Now.Ticks % 100000:D6}";
            var account = new Account
            {
                AccountId = accountId,
                Username = email.Split('@')[0],
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerData.Password, workFactor: 12),
                Role = "Customer",
                Email = email,
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            await _accountRepo.AddAsync(account);

            // Tạo Customer
            var customerId = $"CUS{DateTime.Now.Ticks % 100000:D6}";
            var customer = new Customer
            {
                CustomerId = customerId,
                AccountId = account.AccountId,
                FullName = registerData.FullName.Trim(),
                PhoneNumber = registerData.PhoneNumber.Trim(),
                Gender = registerData.Gender?.Trim(),
                IsDeleted = false
            };
            await _customerRepo.AddAsync(customer);

            // Đánh dấu OTP đã verify
            var otp = await _otpRepo.GetLatestByEmailAsync(email);
            if (otp != null)
            {
                otp.IsVerified = true;
                otp.VerifiedAt = DateTime.UtcNow;
                await _otpRepo.UpdateAsync(otp);
            }

            // Xóa dữ liệu tạm thời
            _registerDataStorage.Remove(email);

            return new RegisterResultDto 
            { 
                Success = true, 
                Message = "Đăng ký thành công! Vui lòng đăng nhập.", 
                Email = email,
                CustomerId = customerId
            };
        }

        /// <summary>
        /// Đăng ký nhân viên thông qua invite token
        /// </summary>
        public async Task<EmployeeRegistrationResultDto> RegisterEmployeeAsync(EmployeeRegisterDto dto)
        {
            // 1. Validate thông tin bắt buộc
            if (string.IsNullOrWhiteSpace(dto.InviteToken) || 
                string.IsNullOrWhiteSpace(dto.Password) || 
                string.IsNullOrWhiteSpace(dto.FullName) ||
                string.IsNullOrWhiteSpace(dto.PhoneNumber) ||
                string.IsNullOrWhiteSpace(dto.Gender))
            {
                return new EmployeeRegistrationResultDto 
                { 
                    Success = false, 
                    Message = "Vui lòng nhập đầy đủ các trường bắt buộc." 
                };
            }

            // 2. Kiểm tra mật khẩu và confirm password
            if (dto.Password != dto.ConfirmPassword)
            {
                return new EmployeeRegistrationResultDto 
                { 
                    Success = false, 
                    Message = "Mật khẩu xác nhận không trùng khớp." 
                };
            }

            // 3. Kiểm tra độ mạnh của password
            if (!IsStrongPassword(dto.Password))
            {
                return new EmployeeRegistrationResultDto 
                { 
                    Success = false, 
                    Message = "Mật khẩu phải có ít nhất 6 ký tự, bao gồm chữ cái, chữ số và ký tự đặc biệt. Ví dụ: Huong@4906" 
                };
            }

            // 4. Kiểm tra invite token hợp lệ
            var invite = await _inviteRepo.GetByTokenAsync(dto.InviteToken);
            if (invite == null)
            {
                return new EmployeeRegistrationResultDto 
                { 
                    Success = false, 
                    Message = "Link invite không hợp lệ hoặc đã hết hạn." 
                };
            }

            // 5. Kiểm tra invite đã được sử dụng
            if (invite.IsUsed)
            {
                return new EmployeeRegistrationResultDto 
                { 
                    Success = false, 
                    Message = "Link invite này đã được sử dụng." 
                };
            }

            // 6. Kiểm tra invite đã hết hạn
            if (DateTime.Now > invite.ExpiryTime)
            {
                return new EmployeeRegistrationResultDto 
                { 
                    Success = false, 
                    Message = "Link invite đã hết hạn." 
                };
            }

            // 7. Kiểm tra email đã tồn tại
            if (await _accountRepo.ExistsEmailAsync(invite.Email))
            {
                return new EmployeeRegistrationResultDto 
                { 
                    Success = false, 
                    Message = "Email này đã được đăng ký." 
                };
            }

            // 8. Tạo Account
            var accountId = $"ACC{DateTime.Now.Ticks % 100000:D6}";
            var account = new Account
            {
                AccountId = accountId,
                Username = invite.Email.Split('@')[0],
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12),
                Role = "Employee",
                Email = invite.Email,
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            await _accountRepo.AddAsync(account);

            // 9. Tạo Employee
            var employeeId = $"EMP{DateTime.Now.Ticks % 100000:D6}";
            var employee = new Employee
            {
                EmployeeId = employeeId,
                AccountId = accountId,
                EmployeeCode = invite.EmployeeCode,
                FullName = dto.FullName.Trim(),
                PhoneNumber = dto.PhoneNumber.Trim(),
                Gender = dto.Gender.Trim(),
                Shift = dto.Shift?.Trim(),
                IsDeleted = false
            };
            await _employeeRepo.AddAsync(employee);

            // 10. Đánh dấu invite đã sử dụng
            invite.IsUsed = true;
            await _inviteRepo.UpdateAsync(invite);

            return new EmployeeRegistrationResultDto 
            { 
                Success = true, 
                Message = "Đăng ký thành công! Vui lòng đăng nhập.", 
                Email = invite.Email,
                EmployeeCode = invite.EmployeeCode,
                EmployeeId = employeeId
            };
        }

        /// <summary>
        /// Gửi OTP xác thực đến email
        /// </summary>
        public async Task<string> SendOtpAsync(string email)
        {
            // Xóa OTP cũ chưa sử dụng
            await _otpRepo.DeleteExpiredAsync();

            // Tạo OTP 6 chữ số
            string otpCode = new Random().Next(100000, 999999).ToString();

            var otpId = await _otpRepo.GenerateIdAsync();
            var otp = new Otp
            {
                OtpId = otpId,
                Email = email.Trim().ToLower(),
                Code = otpCode,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsVerified = false
            };

            await _otpRepo.AddAsync(otp);

            // TODO: Gửi email thực tế
            // Tạm thời in ra console để test
            Console.WriteLine($"OTP cho {email}: {otpCode}");

            return otpCode;
        }

        // ============ HELPER METHODS ============

        /// <summary>
        /// Kiểm tra email hợp lệ
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra mật khẩu an toàn (min 6 ký tự, gồm chữ cái, số, ký tự đặc biệt)
        /// </summary>
        private bool IsStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return false;

            // Kiểm tra chứa chữ cái (a-z hoặc A-Z)
            if (!Regex.IsMatch(password, @"[a-zA-Z]"))
                return false;

            // Kiểm tra chứa số
            if (!Regex.IsMatch(password, @"[0-9]"))
                return false;

            // Kiểm tra chứa ký tự đặc biệt
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};:'""<>,.?/]"))
                return false;

            return true;
        }
    }
}
