using Microsoft.AspNetCore.Mvc;
using ParkingManagement.BLL.DTOs;
using ParkingManagement.BLL.Services.Interfaces;

namespace ParkingManagement.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public IActionResult CheckIn() => View(new CheckInInputDto());

        [HttpPost]
        public async Task<IActionResult> ValidateCheckIn(CheckInInputDto input)
        {
            var result = await _ticketService.ValidateAndPrepareCheckInAsync(input);
            if (!result.AvailableSlots.Any() && result.Message != null)
            {
                ViewBag.Error = result.Message;
                return View("CheckIn", input);
            }
            // Lưu tạm thông tin xe vào session để confirm
            HttpContext.Session.SetString("CheckInVehiclePlate", input.VehiclePlate.ToUpper());
            HttpContext.Session.SetString("CheckInVehicleType", input.VehicleType);
            if (!string.IsNullOrEmpty(result.CustomerId))
                HttpContext.Session.SetString("CheckInCustomerId", result.CustomerId);
            else
                HttpContext.Session.Remove("CheckInCustomerId");

            return View("CheckInConfirm", result);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCheckIn(string slotId)
        {
            var plate = HttpContext.Session.GetString("CheckInVehiclePlate");
            var type = HttpContext.Session.GetString("CheckInVehicleType");
            var customerId = HttpContext.Session.GetString("CheckInCustomerId");

            if(string.IsNullOrEmpty(plate) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(slotId))
            {
                ViewBag.Error = "Dữ liệu không hợp lý. Vui lòng thử lại.";
                return RedirectToAction("CheckIn");
            }

            var dto = new ConfirmCheckInDto
            {
                VehiclePlate = plate,
                VehicleType = type,
                SlotId = slotId,
                CustomerId = customerId
            };

            var result = await _ticketService.ConfirmCheckInAsync(dto);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return RedirectToAction("CheckIn");
            }

            // Clear session
            HttpContext.Session.Remove("CheckInVehiclePlate");
            HttpContext.Session.Remove("CheckInVehicleType");
            HttpContext.Session.Remove("CheckInCustomerId");

            return RedirectToAction("Index", "Home"); 
        }

        [HttpGet]
        public IActionResult CheckOut() => View(new CheckOutInputDto());

        [HttpPost]
        public async Task<IActionResult> ValidateCheckOut(CheckOutInputDto input)
        {
            var result = await _ticketService.ValidateAndPrepareCheckOutAsync(input);
            if (!result.Success && result.Message != null)
            {
                ViewBag.Error = result.Message;
                return View("CheckOut", input);
            }

            HttpContext.Session.SetString("CheckOutTicketId", result.TicketId ?? "");
            HttpContext.Session.SetString("CheckOutFee", result.CalculatedFee.ToString());

            return View("CheckOutConfirm", result);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCheckOut(string paymentMethod)
        {
            var ticketId = HttpContext.Session.GetString("CheckOutTicketId");
            var feeStr = HttpContext.Session.GetString("CheckOutFee");

            if (string.IsNullOrEmpty(ticketId) || !decimal.TryParse(feeStr, out var fee))
            {
                ViewBag.Error = "Dữ liệu không hợp lệ.";
                return RedirectToAction("CheckOut");
            }

            var dto = new ConfirmCheckOutDto
            {
                TicketId = ticketId,
                Fee = fee,
                PaymentMethod = paymentMethod ?? "Tiền mặt"
            };

            var result = await _ticketService.ConfirmCheckOutAsync(dto);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return RedirectToAction("CheckOut");
            }

            // Clear session
            HttpContext.Session.Remove("CheckOutTicketId");
            HttpContext.Session.Remove("CheckOutFee");

            return RedirectToAction("Index", "Home");
        }
    }
}