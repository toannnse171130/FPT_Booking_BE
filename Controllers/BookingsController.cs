using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq; 
using System.Security.Claims;

namespace FPT_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly INotificationService _notiService;

        public BookingsController(IBookingService bookingService, INotificationService notiService)
        {
            _bookingService = bookingService;
            _notiService = notiService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequest request)
        {
            var userIdClaim = User.FindFirst("UserId"); 
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Token không hợp lệ hoặc thiếu thông tin UserID" });
            }

            int userId = int.Parse(userIdClaim.Value);

            var result = await _bookingService.CreateBooking(userId, request);

            if (result != "Success")
            {
                return BadRequest(new { message = result });
            }

            return Ok(new { message = "Gửi yêu cầu đặt phòng thành công!" });
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,FacilityAdmin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] BookingStatusUpdate request)
        {
            if (request.Status != "Approved" && request.Status != "Rejected")
            {
                return BadRequest(new { message = "Trạng thái không hợp lệ. Chỉ chấp nhận 'Approved' hoặc 'Rejected'." });
            }

            var result = await _bookingService.UpdateStatus(id, request.Status, request.RejectionReason);

            switch (result)
            {
                case "Success":
                    return Ok(new { message = $"Đã cập nhật trạng thái thành {request.Status}" });

                case "NotFound":
                    return NotFound(new { message = "Không tìm thấy đơn đặt phòng." });

                case "NotPending":
                    return BadRequest(new { message = "Chỉ có thể duyệt đơn đang ở trạng thái Pending." });

                case "Expired": 
                    return BadRequest(new { message = "Không thể DUYỆT đơn này vì thời gian bắt đầu (Slot) đã trôi qua." });

                default:
                    return StatusCode(500, new { message = "Lỗi hệ thống." });
            }
        }


        [HttpGet("availability")]
        public async Task<IActionResult> CheckAvailability([FromQuery] int facilityId, [FromQuery] DateOnly date)
        {
            var bookedSlots = await _bookingService.GetBookedSlots(facilityId, date);

            return Ok(new
            {
                facilityId = facilityId,
                date = date,
                bookedSlotIds = bookedSlots
            });
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var result = await _bookingService.CancelBooking(userId, id);

            if (result == "Success")
            {
                return Ok(new { message = "Hủy đặt phòng thành công!" });
            }
            else if (result.Contains("quyền")) 
            {
                return Forbid(result);
            }
            else
            {
                return BadRequest(new { message = result });
            }
        }

        [HttpGet("schedule-today")]
        //[Authorize(Roles = "Security,Admin,Manager")]
        public async Task<IActionResult> GetSecuritySchedule([FromQuery] int campusId)
        {
            var schedule = await _bookingService.GetDailyScheduleForSecurity(campusId);
            return Ok(schedule);
        }


        [HttpPost("recurring")]
        public async Task<IActionResult> CreateRecurring([FromBody] BookingRecurringRequest request)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            if (request.EndDate < request.StartDate)
                return BadRequest(new { message = "Ngày kết thúc phải lớn hơn ngày bắt đầu" });

            var result = await _bookingService.CreateRecurringBooking(userId, request);

            return Ok(result);
        }

        [HttpPut("recurring/{recurrenceId}/status")]
        public async Task<IActionResult> UpdateRecurringStatus(string recurrenceId, [FromBody] BookingStatusUpdate request)
        {
            if (request.Status != "Approved" && request.Status != "Rejected")
            {
                return BadRequest(new { message = "Trạng thái không hợp lệ. Chỉ chấp nhận 'Approved' hoặc 'Rejected'." });
            }

            var result = await _bookingService.UpdateRecurringStatus(recurrenceId, request.Status);

            if (result.Contains("Không tìm thấy"))
            {
                return NotFound(new { message = result });
            }

            return Ok(new { message = result });
        }


        [HttpPut("staff-cancel/{id}")]
        public async Task<IActionResult> StaffCancelBooking(int id, [FromBody] StaffCancelRequest request)
        {
            var result = await _bookingService.StaffCancelBookingAsync(id, request.StaffId, request.Reason);

            if (!result)
            {
                return NotFound(new { message = "Không tìm thấy lịch đặt phòng này." });
            }

            return Ok(new { message = "Đã hủy lịch và gửi thông báo thành công!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings([FromQuery] BookingFilterRequest request)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            // Nếu user muốn xem "Lịch sử của tôi" (My History)
            // Hoặc nếu là SV/GV thì mặc định gán UserId vào filter để không xem trộm lịch sử người khác (trừ khi xem lịch phòng công khai)
            bool isViewingPublicSchedule = request.FacilityId.HasValue;

            if (!isViewingPublicSchedule)
            {
                if (role != "Admin" && role != "Manager" && role != "Staff" && role != "FacilityAdmin" && role != "Security")
                {
                    request.UserId = userId;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(request.Status))
                {
                    request.Status = "Approved";
                }
            }

            var result = await _bookingService.GetBookingsFilterAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Get only individual (non-recurring) bookings
        /// </summary>
        [HttpGet("individual")]
        public async Task<IActionResult> GetIndividualBookings([FromQuery] int? userId, [FromQuery] string? status)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            // Non-admin users can only see their own bookings
            if (role != "Admin" && role != "Manager" && role != "Staff" && role != "FacilityAdmin")
            {
                userId = currentUserId;
            }

            var bookings = await _bookingService.GetIndividualBookings(userId, status);
            return Ok(bookings);
        }

        /// <summary>
        /// Get only recurring bookings (individual bookings)
        /// </summary>
        [HttpGet("recurring")]
        public async Task<IActionResult> GetRecurringBookings([FromQuery] int? userId, [FromQuery] string? status)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            // Non-admin users can only see their own bookings
            if (role != "Admin" && role != "Manager" && role != "Staff" && role != "FacilityAdmin")
            {
                userId = currentUserId;
            }

            var bookings = await _bookingService.GetRecurringBookings(userId, status);
            return Ok(bookings);
        }

        /// <summary>
        /// Get recurring bookings grouped by series (ONE entry per recurring series)
        /// Perfect for managers to approve/view recurring booking series
        /// </summary>
        [HttpGet("recurring-groups")]
        public async Task<IActionResult> GetRecurringBookingGroups([FromQuery] int? userId)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            // Non-admin users can only see their own bookings
            if (role != "Admin" && role != "Manager" && role != "Staff" && role != "FacilityAdmin")
            {
                userId = currentUserId;
            }

            var groups = await _bookingService.GetRecurringBookingGroupsAsync(userId);
            return Ok(groups);
        }

        /// <summary>
        /// Get all bookings that belong to a specific recurring group
        /// </summary>
        [HttpGet("recurring-group/{recurrenceGroupId}")]
        public async Task<IActionResult> GetBookingsByRecurringGroupId(string recurrenceGroupId)
        {
            var bookings = await _bookingService.GetBookingsByRecurringGroupId(recurrenceGroupId);
            return Ok(bookings);
        }

        /// <summary>
        /// Get total count of all bookings
        /// </summary>
        [HttpGet("count")]
        public async Task<IActionResult> GetTotalBookingsCount(int? userId)
        {
            var count = await _bookingService.GetTotalBookingsCount(userId);
            return Ok(new { totalBookings = count });
        }

        /// <summary>
        /// Check if there's a booking conflict for a specific facility, date, and slot
        /// Returns conflict details including the user who booked it and whether you can override
        /// </summary>
        [HttpPost("check-conflict")]
        public async Task<IActionResult> CheckBookingConflict([FromBody] BookingCreateRequest request)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Token không hợp lệ" });
            }

            int userId = int.Parse(userIdClaim.Value);
            var conflict = await _bookingService.CheckBookingConflict(userId, request.FacilityId, request.BookingDate, request.SlotId);

            if (conflict == null)
            {
                return Ok(new { hasConflict = false, message = "Phòng trống, có thể đặt" });
            }

            return Ok(new { hasConflict = true, conflict });
        }

        /// <summary>
        /// Check conflicts for recurring booking across all dates
        /// Returns detailed conflict information for each date in the recurring pattern
        /// </summary>
        [HttpPost("check-recurring-conflicts")]
        public async Task<IActionResult> CheckRecurringConflicts([FromBody] BookingRecurringRequest request)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Token không hợp lệ" });
            }

            int userId = int.Parse(userIdClaim.Value);
            var result = await _bookingService.CheckRecurringConflicts(userId, request);

            return Ok(result);
        }

    }
}   