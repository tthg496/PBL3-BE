using Microsoft.AspNetCore.Mvc;
using ParkingManagement.BLL.DTOs;
using ParkingManagement.BLL.Services.Interfaces;

namespace ParkingManagement.Web.Controllers.Api
{
    /// <summary>
    /// API for Monthly Ticket Management
    /// </summary>
    [ApiController]
    [Route("api/monthly-tickets")]
    [Produces("application/json")]
    public class MonthlyTicketsController : ControllerBase
    {
        private readonly IMonthlyTicketService _monthlyTicketService;

        public MonthlyTicketsController(IMonthlyTicketService monthlyTicketService)
        {
            _monthlyTicketService = monthlyTicketService;
        }

        /// <summary>
        /// Get all monthly tickets
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<MonthlyTicketDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _monthlyTicketService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get monthly tickets by customer
        /// </summary>
        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(List<MonthlyTicketDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var result = await _monthlyTicketService.GetByCustomerIdAsync(customerId);
            return Ok(result);
        }

        /// <summary>
        /// Get monthly ticket by ID
        /// </summary>
        [HttpGet("{monthlyTicketId}")]
        [ProducesResponseType(typeof(MonthlyTicketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string monthlyTicketId)
        {
            var result = await _monthlyTicketService.GetByIdAsync(monthlyTicketId);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy vé tháng" });
            return Ok(result);
        }

        /// <summary>
        /// Create new monthly ticket
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<MonthlyTicketDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ServiceResult<MonthlyTicketDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] object dto)
        {
            // TODO: Implement CreateMonthlyTicketDto and CreateAsync method in service
            return StatusCode(StatusCodes.Status501NotImplemented, "Not yet implemented");
        }

        /// <summary>
        /// Update monthly ticket
        /// </summary>
        [HttpPut("{monthlyTicketId}")]
        [ProducesResponseType(typeof(ServiceResult<MonthlyTicketDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<MonthlyTicketDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string monthlyTicketId, [FromBody] object dto)
        {
            // TODO: Implement UpdateMonthlyTicketDto and UpdateAsync method in service
            return StatusCode(StatusCodes.Status501NotImplemented, "Not yet implemented");
        }

        /// <summary>
        /// Cancel/Deactivate monthly ticket
        /// </summary>
        [HttpDelete("{monthlyTicketId}")]
        [ProducesResponseType(typeof(ServiceResult<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Cancel(string monthlyTicketId)
        {
            // TODO: Implement CancelAsync method in service
            return StatusCode(StatusCodes.Status501NotImplemented, "Not yet implemented");
        }

        /// <summary>
        /// Renew monthly ticket
        /// </summary>
        [HttpPost("{monthlyTicketId}/renew")]
        [ProducesResponseType(typeof(ServiceResult<MonthlyTicketDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<MonthlyTicketDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Renew(string monthlyTicketId, [FromBody] object dto)
        {
            // TODO: Implement RenewMonthlyTicketDto and RenewAsync method in service
            return StatusCode(StatusCodes.Status501NotImplemented, "Not yet implemented");
        }
    }
}
