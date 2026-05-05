using Microsoft.AspNetCore.Mvc;
using ParkingManagement.BLL.DTOs;
using ParkingManagement.BLL.Services.Interfaces;

namespace ParkingManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/Customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _customerService.GetByIdAsync(id);
            if (result == null) return NotFound("Khách hàng không tồn tại.");
            return Ok(result);
        }

        // GET: api/Customers/search
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var result = await _customerService.SearchAsync(keyword);
            return Ok(result);
        }

        // POST: api/Customers/search-advanced
        [HttpPost("search-advanced")]
        public async Task<IActionResult> SearchAdvanced([FromBody] CustomerSearchDto searchDto)
        {
            var result = await _customerService.SearchAdvancedAsync(searchDto);
            return Ok(result);
        }

        // POST: api/Customers/employee-search
        [HttpPost("employee-search")]
        public async Task<IActionResult> EmployeeSearchCustomers([FromBody] EmployeeCustomerSearchFilterDto filter)
        {
            var result = await _customerService.SearchCustomersAsync(filter);
            return Ok(result);
        }

        // GET: api/Customers/{id}/details
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetCustomerDetail(string id)
        {
            try
            {
                var result = await _customerService.GetCustomerDetailAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _customerService.SoftDeleteAsync(id);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result);
        }

        // POST: api/Customers/{id}/restore
        [HttpPost("{id}/restore")]
        public async Task<IActionResult> Restore(string id)
        {
            var result = await _customerService.RestoreAsync(id);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result);
        }
    }
}
