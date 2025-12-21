using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly FptFacilityBookingContext _context;

        public BookingRepository(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task AddBooking(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }
        public async Task<Booking?> GetConflictingBooking(int facilityId, DateOnly date, int slotId)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(b =>
                    b.FacilityId == facilityId &&
                    b.BookingDate == date &&
                    b.SlotId == slotId &&
                    b.Status != "Cancelled" &&
                    b.Status != "Pending" &&
                    b.Status != "Rejected"
                );
        }

        public async Task<IEnumerable<Booking>> GetBookings(int? userId, string? status)
        {
            var query = _context.Bookings
                .Include(b => b.Facility)
                .Include(b => b.Slot)
                .Include(b => b.User)
                .AsQueryable();

            if (userId.HasValue) query = query.Where(b => b.UserId == userId);
            if (!string.IsNullOrEmpty(status)) query = query.Where(b => b.Status == status);

            return await query.OrderByDescending(b => b.BookingDate).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetIndividualBookings(int? userId, string? status)
        {
            var query = _context.Bookings
                .Include(b => b.Facility)
                .Include(b => b.Slot)
                .Include(b => b.User)
                .Where(b => string.IsNullOrEmpty(b.RecurrenceGroupId) || b.BookingType == "Individual")
                .AsQueryable();

            if (userId.HasValue) query = query.Where(b => b.UserId == userId);
            if (!string.IsNullOrEmpty(status)) query = query.Where(b => b.Status == status);

            return await query.OrderByDescending(b => b.BookingDate).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetRecurringBookings(int? userId, string? status)
        {
            var query = _context.Bookings
                .Include(b => b.Facility)
                .Include(b => b.Slot)
                .Include(b => b.User)
                .Where(b => !string.IsNullOrEmpty(b.RecurrenceGroupId) && b.BookingType == "Group")
                .AsQueryable();

            if (userId.HasValue) query = query.Where(b => b.UserId == userId);
            if (!string.IsNullOrEmpty(status)) query = query.Where(b => b.Status == status);

            return await query.OrderByDescending(b => b.BookingDate).ToListAsync();
        }

        public async Task<IEnumerable<IGrouping<string, Booking>>> GetRecurringBookingGroupsAsync(int? userId)
        {
            var query = _context.Bookings
                .Include(b => b.Facility)
                .Include(b => b.Slot)
                .Include(b => b.User)
                .ThenInclude(u => u.Role)
                .Where(b => !string.IsNullOrEmpty(b.RecurrenceGroupId) && b.BookingType == "Group")
                .AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(b => b.UserId == userId);
            }

            var bookings = await query.ToListAsync();
            return bookings.GroupBy(b => b.RecurrenceGroupId!);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByRecurringGroupId(string recurrenceGroupId)
        {
            return await _context.Bookings
                .Include(b => b.Facility)
                .ThenInclude(f => f.Type)
                .Include(b => b.Slot)
                .Include(b => b.User)
                .ThenInclude(u => u.Role)
                .Where(b => b.RecurrenceGroupId == recurrenceGroupId)
                .OrderBy(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingById(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task UpdateBooking(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<int>> GetBookedSlotIds(int facilityId, DateOnly date)
        {
            return await _context.Bookings
                .Where(b => b.FacilityId == facilityId &&
                            b.BookingDate == date &&
                            b.Status != "Cancelled" &&
                            b.Status != "Rejected")
                .Select(b => b.SlotId) 
                .Distinct()            
                .ToListAsync();
        }

        public async Task<bool> IsBookingConflict(int facilityId, DateOnly bookingDate, int slotId)
        {
            return await _context.Bookings.AnyAsync(b =>
                b.FacilityId == facilityId &&
                b.BookingDate == bookingDate &&
                b.SlotId == slotId &&
                b.Status != "Cancelled" && b.Status != "Rejected"
            );
        }

        public async Task<Booking?> GetConflictingBooking2(int facilityId, DateOnly bookingDate, int slotId)
        {
            return await _context.Bookings
                .Include(b => b.User).ThenInclude(u => u.Role) 
                .FirstOrDefaultAsync(b =>
                    b.FacilityId == facilityId &&
                    b.BookingDate == bookingDate &&
                    b.SlotId == slotId &&
                    b.Status != "Cancelled" && b.Status != "Rejected"
                );
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                 //.Include(b => b.Purpose)
                 .Include(b => b.User) 
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalBookingsCount()
        {
            return await _context.Bookings.CountAsync();
        }
        public async Task<int> GetTotalBookingsCountByUser(int userId)
        {
            return await _context.Bookings.CountAsync(b => b.UserId == userId);
        }
    }
}