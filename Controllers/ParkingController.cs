using Microsoft.AspNetCore.Mvc;
using ParkingManagement.BLL.Interfaces;
using ParkingManagement.Web.Models.Requests;
using ParkingManagement.Web.Models.Responses;

namespace ParkingManagement.Web.Controllers;

/// <summary>
/// API quản lý vào/ra bãi đỗ xe.
/// </summary>
[ApiController]
[Route("api/old/parking")]
[Produces("application/json")]
public class ParkingController(IParkingService parkingService) : ControllerBase
{
    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>Ghi nhận xe vào bãi (Check-In).</summary>
    /// <remarks>
    /// Ví dụ request:
    ///
    ///     POST /api/parking/check-in
    ///     {
    ///         "licensePlate": "43A-12345",
    ///         "vehicleType": "Motorcycle",
    ///         "areaId": 1
    ///     }
    ///
    /// **VehicleType** hợp lệ: `Motorcycle`, `Car`, `Truck`, `Bicycle`
    /// </remarks>
    /// <response code="200">Vé mới được tạo thành công.</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ.</response>
    /// <response code="409">Bãi đầy hoặc xe đang có vé chưa checkout.</response>
    [HttpPost("check-in")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CheckIn([FromBody] CheckInRequest request)
    {
        try
        {
            var result = await parkingService.CheckInAsync(
                request.LicensePlate,
                request.VehicleType,
                request.AreaId);

            return Ok(ApiResponse<object>.Ok(result,
                $"Xe {result.LicensePlate} đã vào bãi '{result.AreaName}'. Mã vé: #{result.TicketId}"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<object>.Fail(ex.Message));
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>Ghi nhận xe ra khỏi bãi và tính tiền (Check-Out).</summary>
    /// <remarks>
    /// Ví dụ request:
    ///
    ///     POST /api/parking/check-out
    ///     {
    ///         "ticketId": 1
    ///     }
    ///
    /// Thời gian được làm tròn lên (Math.Ceiling), tối thiểu 1 giờ.
    /// Giá: Xe máy/Xe đạp **5.000đ/giờ** · Ô tô/Tải **20.000đ/giờ**.
    /// </remarks>
    /// <response code="200">Checkout thành công, trả về thông tin tiền và thời gian.</response>
    /// <response code="400">Trạng thái vé không hợp lệ.</response>
    /// <response code="404">Không tìm thấy vé.</response>
    [HttpPost("check-out")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckOut([FromBody] CheckOutRequest request)
    {
        try
        {
            var result = await parkingService.CheckOutAsync(request.TicketId);

            return Ok(ApiResponse<object>.Ok(result,
                $"Xe {result.LicensePlate} đã ra bãi. " +
                $"Thời gian: {result.DurationHours} giờ. " +
                $"Phí: {result.Price:N0}đ"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }
}
