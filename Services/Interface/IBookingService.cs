using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Services
{
    public interface IBookingService
    {
        Task<string> CreateBooking(int userId, BookingCreateRequest request);

        Task<List<BookingHistoryDto>> GetHistory(int userId);
        Task<List<BookingHistoryDto>> GetIndividualBookings(int? userId, string? status);
        Task<List<BookingHistoryDto>> GetRecurringBookings(int? userId, string? status);
        Task<List<RecurringBookingGroupDto>> GetRecurringBookingGroupsAsync(int? userId);
        Task<List<BookingHistoryDto>> GetBookingsByRecurringGroupId(string recurrenceGroupId);

        Task<string> UpdateStatus(int bookingId, string status, string? rejectionReason);
        Task<List<int>> GetBookedSlots(int facilityId, DateOnly date);

        Task<string> CancelBooking(int userId, int bookingId);

        Task<List<BookingHistoryDto>> GetDailyScheduleForSecurity(int campusId);

        Task<object> CreateRecurringBooking(int userId, BookingRecurringRequest request);

        Task<string> UpdateRecurringStatus(string recurrenceId, string status);

        Task<bool> StaffCancelBookingAsync(int bookingId, int staffId, string reason);

        Task<PagedResult<BookingResponse>> GetBookingsFilterAsync(BookingFilterRequest request);
        
        Task<int> GetTotalBookingsCount(int? userId);

        Task<BookingConflictDto?> CheckBookingConflict(int userId, int facilityId, DateOnly bookingDate, int slotId);

        Task<RecurringConflictCheckResponse> CheckRecurringConflicts(int userId, BookingRecurringRequest request);
    }
}