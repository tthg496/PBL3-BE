using Microsoft.AspNetCore.Mvc;
using ParkingManagement.BLL.Services.Interfaces;

namespace ParkingManagement.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _service;
        private readonly IParkingSlotService _slotService;

        public ReportController(IReportService service, IParkingSlotService slotService)
        {
            _service = service;
            _slotService = slotService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var summary = await _slotService.GetSlotSummaryAsync();
            ViewBag.SlotSummary = summary;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Revenue(DateTime from, DateTime to)
        {
            var report = await _service.GetRevenueReportAsync(from, to);
            return View("RevenueResult", report);
        }
    }
}