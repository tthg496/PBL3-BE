using Microsoft.AspNetCore.Mvc;
using ParkingManagement.BLL.DTOs;
using ParkingManagement.BLL.Services.Interfaces;

namespace ParkingManagement.Web.Controllers.Api
{
    /// <summary>
    /// API for Authentication (REST)
    /// Handles login, registration, token management for mobile/web clients
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class ApiAuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public ApiAuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ServiceResult<LoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.Success)
                return Unauthorized(result);

            var response = new ServiceResult<LoginResponseDto>
            {
                Success = true,
                Message = result.Message,
                Data = new LoginResponseDto
                {
                    AccountId = result.AccountId,
                    Role = result.Role,
                    FullName = result.FullName,
                    RelatedId = result.RelatedId,
                    Token = GenerateJwtToken(result.AccountId, result.Role) // TODO: Implement JWT
                }
            };

            return Ok(response);
        }

        /// <summary>
        /// Customer registration
        /// </summary>
        [HttpPost("register/customer")]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            // Send OTP via email
            await _emailService.SendOtpEmailAsync(dto.Email, dto.FullName, result.Otp);

            return CreatedAtAction(nameof(VerifyOtp), result);
        }

        /// <summary>
        /// Employee registration via invite link
        /// </summary>
        [HttpPost("register/employee")]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeRegisterDto dto)
        {
            var result = await _authService.RegisterEmployeeAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(Login), result);
        }

        /// <summary>
        /// Verify OTP after registration
        /// </summary>
        [HttpPost("verify-otp")]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var result = await _authService.VerifyOtpAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Refresh authentication token
        /// </summary>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] object tokenRequest)
        {
            // TODO: Implement RefreshTokenAsync in service
            return StatusCode(StatusCodes.Status501NotImplemented, "Not yet implemented");
        }

        /// <summary>
        /// Logout (invalidate token)
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            // TODO: Implement token blacklisting
            return Ok(new { message = "Logout successful" });
        }

        /// <summary>
        /// Request password reset
        /// </summary>
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            // TODO: Implement ForgotPasswordAsync in service
            return StatusCode(StatusCodes.Status501NotImplemented, "Not yet implemented");
        }

        /// <summary>
        /// Reset password with token
        /// </summary>
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] object resetRequest)
        {
            // TODO: Implement ResetPasswordAsync in service
            return StatusCode(StatusCodes.Status501NotImplemented, "Not yet implemented");
        }

        /// <summary>
        /// Change password (authenticated user)
        /// </summary>
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] object changePasswordRequest)
        {
            // TODO: Implement ChangePasswordAsync in service
            return StatusCode(StatusCodes.Status501NotImplemented, "Not yet implemented");
        }

        /// <summary>
        /// Get current user profile
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            // TODO: Get from JWT claims or session
            return StatusCode(StatusCodes.Status501NotImplemented, "Not yet implemented");
        }

        // TODO: Implement JWT token generation
        private string GenerateJwtToken(string accountId, string role)
        {
            // This is a placeholder - implement actual JWT generation
            return $"jwt_token_for_{accountId}_{role}";
        }
    }

    /// <summary>
    /// Login response DTO
    /// </summary>
    public class LoginResponseDto
    {
        public string AccountId { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string RelatedId { get; set; }
        public string Token { get; set; }
    }
}
