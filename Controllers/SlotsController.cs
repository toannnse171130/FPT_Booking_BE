using FPT_Booking_BE.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FPT_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SlotsController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotsController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? facilityId,
            [FromQuery] DateOnly? date)
        {
            var slots = await _slotService.GetAllSlots(facilityId, date);
            return Ok(slots);
        }
    }
}