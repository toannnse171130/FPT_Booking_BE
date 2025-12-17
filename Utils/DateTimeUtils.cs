using System;

namespace FPT_Booking_BE.Utils
{
    /// <summary>
    /// Utility class for date and time operations
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// Gets the day of week number in Vietnamese format (Monday = 2, Sunday = 8)
        /// </summary>
        public static int GetVietnameseDayOfWeek(DateOnly date)
        {
            // C# DayOfWeek: Sunday = 0, Monday = 1, ..., Saturday = 6
            // Vietnamese: Monday = 2, Tuesday = 3, ..., Sunday = 8
            return (int)date.DayOfWeek == 0 ? 8 : (int)date.DayOfWeek + 1;
        }

        /// <summary>
        /// Gets the start of the week (Monday) for a given date
        /// </summary>
        public static DateOnly GetStartOfWeek(DateOnly date)
        {
            int daysFromMonday = ((int)date.DayOfWeek + 6) % 7; // 0 = Monday
            return date.AddDays(-daysFromMonday);
        }

        /// <summary>
        /// Gets the end of the week (Sunday) for a given date
        /// </summary>
        public static DateOnly GetEndOfWeek(DateOnly date)
        {
            return GetStartOfWeek(date).AddDays(6);
        }

        /// <summary>
        /// Gets the start of the month for a given date
        /// </summary>
        public static DateOnly GetStartOfMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Gets the end of the month for a given date
        /// </summary>
        public static DateOnly GetEndOfMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        /// <summary>
        /// Gets the week number in a month (1-based)
        /// </summary>
        public static int GetWeekOfMonth(DateOnly date)
        {
            var firstDayOfMonth = new DateOnly(date.Year, date.Month, 1);
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            
            // Adjust so Monday is the start of the week
            int offset = (firstDayOfWeek == 0) ? 6 : firstDayOfWeek - 1;
            
            return (date.Day + offset) / 7 + 1;
        }

        /// <summary>
        /// Checks if a date is a weekend (Saturday or Sunday)
        /// </summary>
        public static bool IsWeekend(DateOnly date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Checks if a date is a weekday (Monday to Friday)
        /// </summary>
        public static bool IsWeekday(DateOnly date)
        {
            return !IsWeekend(date);
        }

        /// <summary>
        /// Gets the name of the day in Vietnamese
        /// </summary>
        public static string GetVietnameseDayName(DateOnly date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Monday => "Thứ Hai",
                DayOfWeek.Tuesday => "Thứ Ba",
                DayOfWeek.Wednesday => "Thứ Tư",
                DayOfWeek.Thursday => "Thứ Năm",
                DayOfWeek.Friday => "Thứ Sáu",
                DayOfWeek.Saturday => "Thứ Bảy",
                DayOfWeek.Sunday => "Chủ Nhật",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Adds weeks to a date
        /// </summary>
        public static DateOnly AddWeeks(DateOnly date, int weeks)
        {
            return date.AddDays(weeks * 7);
        }

        /// <summary>
        /// Adds months to a date
        /// </summary>
        public static DateOnly AddMonths(DateOnly date, int months)
        {
            return date.AddMonths(months);
        }

        /// <summary>
        /// Gets all dates in a date range
        /// </summary>
        public static List<DateOnly> GetDateRange(DateOnly startDate, DateOnly endDate)
        {
            var dates = new List<DateOnly>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                dates.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }

            return dates;
        }

        /// <summary>
        /// Filters dates by specific days of week (Vietnamese format)
        /// </summary>
        public static List<DateOnly> FilterByDaysOfWeek(List<DateOnly> dates, List<int> daysOfWeek)
        {
            return dates.Where(d => daysOfWeek.Contains(GetVietnameseDayOfWeek(d))).ToList();
        }
    }
}
