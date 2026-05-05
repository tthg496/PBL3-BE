using Microsoft.AspNetCore.Mvc;
using ParkingManagement.BLL.DTOs;
using ParkingManagement.BLL.Services.Interfaces;

namespace ParkingManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // ── 1. General (Employee & Manager) ──

        [HttpGet]
        public async Task<IActionResult> GetTickets([FromQuery] TicketFilterDto filter)
        {
            var result = await _ticketService.GetTicketsAsync(filter);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketDetail(string id)
        {
            try
            {
                var result = await _ticketService.GetTicketDetailAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTickets([FromQuery] EmployeeTicketSearchDto search)
        {
            var result = await _ticketService.SearchTicketsAsync(search);
            return Ok(result);
        }

        // ── 2. Customer Specific ──

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerTickets(string customerId, [FromQuery] CustomerTicketFilterDto filter)
        {
            var result = await _ticketService.GetMyTicketsAsync(customerId, filter);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}/detail/{ticketId}")]
        public async Task<IActionResult> GetCustomerTicketDetail(string customerId, string ticketId)
        {
            try
            {
                var result = await _ticketService.GetCustomerTicketDetailAsync(customerId, ticketId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customer/{customerId}/payments")]
        public async Task<IActionResult> GetCustomerPayments(string customerId, [FromQuery] CustomerPaymentFilterDto filter)
        {
            var result = await _ticketService.GetPaymentHistoryAsync(customerId, filter);
            return Ok(result);
        }

        // ── 3. Check-in ──

        [HttpPost("checkin/validate")]
        public async Task<IActionResult> ValidateCheckIn([FromBody] CheckInInputDto input)
        {
            var result = await _ticketService.ValidateAndPrepareCheckInAsync(input);
            return Ok(result);
        }

        [HttpPost("checkin/confirm")]
        public async Task<IActionResult> ConfirmCheckIn([FromBody] ConfirmCheckInDto input)
        {
            var result = await _ticketService.ConfirmCheckInAsync(input);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        // ── 4. Check-out ──

        [HttpPost("checkout/validate")]
        public async Task<IActionResult> ValidateCheckOut([FromBody] CheckOutInputDto input)
        {
            var result = await _ticketService.ValidateAndPrepareCheckOutAsync(input);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("checkout/confirm")]
        public async Task<IActionResult> ConfirmCheckOut([FromBody] ConfirmCheckOutDto input)
        {
            var result = await _ticketService.ConfirmCheckOutAsync(input);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
