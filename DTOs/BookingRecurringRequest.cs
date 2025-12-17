using System;
using System.Collections.Generic;
using FPT_Booking_BE.Utils;

namespace FPT_Booking_BE.DTOs
{
    public class BookingRecurringRequest
    {
        public int FacilityId { get; set; }
        public int SlotId { get; set; }
        public string Purpose { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Recurrence pattern (Daily, Weekly, Weekdays, Weekends, Monthly, Custom)
        /// </summary>
        public RecurrencePattern Pattern { get; set; } = RecurrencePattern.Weekly;

        /// <summary>
        /// Days of week in Vietnamese format (2=Monday, 3=Tuesday, ..., 8=Sunday)
        /// Only used when Pattern is Custom or Weekly
        /// </summary>
        public List<int> DaysOfWeek { get; set; } = new List<int>();

        /// <summary>
        /// Interval for repetition (e.g., every 2 weeks, every 3 days)
        /// Default is 1
        /// </summary>
        public int Interval { get; set; } = 1;

        /// <summary>
        /// Whether to auto-find alternative rooms if the requested room is unavailable
        /// </summary>
        public bool AutoFindAlternative { get; set; } = true;

        /// <summary>
        /// Whether to skip conflicts or fail the entire operation
        /// </summary>
        public bool SkipConflicts { get; set; } = true;
    }
}