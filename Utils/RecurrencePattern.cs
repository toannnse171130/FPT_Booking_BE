namespace FPT_Booking_BE.Utils
{
    /// <summary>
    /// Defines the pattern for recurring bookings
    /// </summary>
    public enum RecurrencePattern
    {
        /// <summary>
        /// Repeat every day
        /// </summary>
        Daily = 1,

        /// <summary>
        /// Repeat every week on the same day(s)
        /// </summary>
        Weekly = 2,

        /// <summary>
        /// Repeat on weekdays only (Monday to Friday)
        /// </summary>
        Weekdays = 3,

        /// <summary>
        /// Repeat on weekends only (Saturday and Sunday)
        /// </summary>
        Weekends = 4,

        /// <summary>
        /// Repeat every month on the same date
        /// </summary>
        Monthly = 5,

        /// <summary>
        /// Custom pattern - specify which days of week
        /// </summary>
        Custom = 6
    }
}
