using Microsoft.AspNetCore.Mvc;
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Utils;

namespace FPT_Booking_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly FptFacilityBookingContext _context;

        public SeedController(FptFacilityBookingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Seed test booking data
        /// </summary>
        [HttpPost("test-bookings")]
        public async Task<IActionResult> SeedTestBookings()
        {
            try
            {
                await DatabaseSeeder.SeedTestData(_context);
                return Ok(new { success = true, message = "Test booking data seeded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Seed conflict scenario bookings
        /// </summary>
        [HttpPost("conflict-scenarios")]
        public async Task<IActionResult> SeedConflictScenarios()
        {
            try
            {
                await DatabaseSeeder.SeedConflictScenarios(_context);
                return Ok(new { success = true, message = "Conflict scenarios seeded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Seed all test data at once
        /// </summary>
        [HttpPost("all")]
        public async Task<IActionResult> SeedAll()
        {
            try
            {
                await DatabaseSeeder.SeedTestData(_context);
                await DatabaseSeeder.SeedConflictScenarios(_context);
                return Ok(new { success = true, message = "All test data seeded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
