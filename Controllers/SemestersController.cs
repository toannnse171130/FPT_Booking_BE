using Microsoft.AspNetCore.Mvc;
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Services;

namespace FPT_Booking_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemestersController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemestersController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        /// <summary>
        /// Get all semesters
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllSemesters()
        {
            try
            {
                var semesters = await _semesterService.GetAllSemestersAsync();
                return Ok(new { success = true, data = semesters });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get current active semester
        /// </summary>
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentSemester()
        {
            try
            {
                var semester = await _semesterService.GetCurrentSemesterAsync();
                if (semester == null)
                {
                    return NotFound(new { success = false, message = "Không có học kỳ đang hoạt động" });
                }
                return Ok(new { success = true, data = semester });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get semester by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSemesterById(int id)
        {
            try
            {
                var semester = await _semesterService.GetSemesterByIdAsync(id);
                if (semester == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy học kỳ" });
                }
                return Ok(new { success = true, data = semester });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Create new semester
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSemester([FromBody] Semester semester)
        {
            try
            {
                var createdSemester = await _semesterService.CreateSemesterAsync(semester);
                return Ok(new { success = true, message = "Tạo học kỳ thành công", data = createdSemester });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Update semester
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSemester(int id, [FromBody] Semester semester)
        {
            try
            {
                var updatedSemester = await _semesterService.UpdateSemesterAsync(id, semester);
                if (updatedSemester == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy học kỳ" });
                }
                return Ok(new { success = true, message = "Cập nhật học kỳ thành công", data = updatedSemester });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete semester
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemester(int id)
        {
            try
            {
                var result = await _semesterService.DeleteSemesterAsync(id);
                if (!result)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy học kỳ" });
                }
                return Ok(new { success = true, message = "Xóa học kỳ thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get semesters within a date range
        /// </summary>
        [HttpGet("range")]
        public async Task<IActionResult> GetSemestersInRange([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            try
            {
                var semesters = await _semesterService.GetSemestersInRangeAsync(startDate, endDate);
                return Ok(new { success = true, data = semesters, count = semesters.Count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
