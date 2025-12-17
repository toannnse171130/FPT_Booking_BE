using System;
using System.Collections.Generic;
using System.Linq;
using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Utils
{
    /// <summary>
    /// Utility class for handling recurring booking operations
    /// </summary>
    public static class RecurringBookingUtils
    {
        /// <summary>
        /// Generates all booking dates based on the recurrence pattern
        /// </summary>
        public static List<DateOnly> GenerateRecurringDates(BookingRecurringRequest request)
        {
            var dates = new List<DateOnly>();

            switch (request.Pattern)
            {
                case RecurrencePattern.Daily:
                    dates = GenerateDailyDates(request.StartDate, request.EndDate, request.Interval);
                    break;

                case RecurrencePattern.Weekly:
                    dates = GenerateWeeklyDates(request.StartDate, request.EndDate, request.DaysOfWeek, request.Interval);
                    break;

                case RecurrencePattern.Weekdays:
                    dates = GenerateWeekdayDates(request.StartDate, request.EndDate);
                    break;

                case RecurrencePattern.Weekends:
                    dates = GenerateWeekendDates(request.StartDate, request.EndDate);
                    break;

                case RecurrencePattern.Monthly:
                    dates = GenerateMonthlyDates(request.StartDate, request.EndDate, request.Interval);
                    break;

                case RecurrencePattern.Custom:
                    dates = GenerateCustomDates(request.StartDate, request.EndDate, request.DaysOfWeek);
                    break;
            }

            return dates;
        }

        /// <summary>
        /// Generates daily recurring dates
        /// </summary>
        private static List<DateOnly> GenerateDailyDates(DateOnly startDate, DateOnly endDate, int interval)
        {
            var dates = new List<DateOnly>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                dates.Add(currentDate);
                currentDate = currentDate.AddDays(interval);
            }

            return dates;
        }

        /// <summary>
        /// Generates weekly recurring dates for specific days of week
        /// </summary>
        private static List<DateOnly> GenerateWeeklyDates(DateOnly startDate, DateOnly endDate, List<int> daysOfWeek, int interval)
        {
            var dates = new List<DateOnly>();
            
            if (daysOfWeek == null || daysOfWeek.Count == 0)
            {
                // If no specific days, use the start date's day of week
                daysOfWeek = new List<int> { DateTimeUtils.GetVietnameseDayOfWeek(startDate) };
            }

            var currentWeekStart = DateTimeUtils.GetStartOfWeek(startDate);
            int weekCount = 0;

            while (currentWeekStart <= endDate)
            {
                // Check each day in the current week
                for (int i = 0; i < 7; i++)
                {
                    var date = currentWeekStart.AddDays(i);
                    
                    if (date >= startDate && date <= endDate)
                    {
                        int vietnameseDay = DateTimeUtils.GetVietnameseDayOfWeek(date);
                        if (daysOfWeek.Contains(vietnameseDay))
                        {
                            dates.Add(date);
                        }
                    }
                }

                // Move to next week based on interval
                weekCount++;
                currentWeekStart = currentWeekStart.AddDays(7 * interval);
            }

            return dates.OrderBy(d => d).ToList();
        }

        /// <summary>
        /// Generates dates for weekdays only (Monday to Friday)
        /// </summary>
        private static List<DateOnly> GenerateWeekdayDates(DateOnly startDate, DateOnly endDate)
        {
            var dates = new List<DateOnly>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                if (DateTimeUtils.IsWeekday(currentDate))
                {
                    dates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(1);
            }

            return dates;
        }

        /// <summary>
        /// Generates dates for weekends only (Saturday and Sunday)
        /// </summary>
        private static List<DateOnly> GenerateWeekendDates(DateOnly startDate, DateOnly endDate)
        {
            var dates = new List<DateOnly>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                if (DateTimeUtils.IsWeekend(currentDate))
                {
                    dates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(1);
            }

            return dates;
        }

        /// <summary>
        /// Generates monthly recurring dates on the same day of month
        /// </summary>
        private static List<DateOnly> GenerateMonthlyDates(DateOnly startDate, DateOnly endDate, int interval)
        {
            var dates = new List<DateOnly>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                dates.Add(currentDate);
                
                // Try to add months
                try
                {
                    currentDate = currentDate.AddMonths(interval);
                }
                catch
                {
                    // Handle cases where the day doesn't exist in target month (e.g., Jan 31 -> Feb)
                    var nextMonth = currentDate.AddMonths(interval);
                    var lastDayOfMonth = DateTimeUtils.GetEndOfMonth(nextMonth);
                    currentDate = lastDayOfMonth;
                }
            }

            return dates;
        }

        /// <summary>
        /// Generates custom recurring dates based on specific days of week
        /// </summary>
        private static List<DateOnly> GenerateCustomDates(DateOnly startDate, DateOnly endDate, List<int> daysOfWeek)
        {
            if (daysOfWeek == null || daysOfWeek.Count == 0)
            {
                return new List<DateOnly>();
            }

            var dates = new List<DateOnly>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                int vietnameseDay = DateTimeUtils.GetVietnameseDayOfWeek(currentDate);
                if (daysOfWeek.Contains(vietnameseDay))
                {
                    dates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(1);
            }

            return dates;
        }

        /// <summary>
        /// Validates the recurring booking request
        /// </summary>
        public static (bool isValid, string errorMessage) ValidateRecurringRequest(BookingRecurringRequest request)
        {
            if (request.StartDate > request.EndDate)
            {
                return (false, "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.");
            }

            if (request.StartDate < DateOnly.FromDateTime(DateTime.Now))
            {
                return (false, "Ngày bắt đầu không thể là quá khứ.");
            }

            if (request.Interval < 1)
            {
                return (false, "Khoảng cách lặp lại phải lớn hơn 0.");
            }

            if (request.Pattern == RecurrencePattern.Custom && (request.DaysOfWeek == null || request.DaysOfWeek.Count == 0))
            {
                return (false, "Với mẫu Custom, bạn phải chọn ít nhất một ngày trong tuần.");
            }

            if (request.Pattern == RecurrencePattern.Weekly && (request.DaysOfWeek == null || request.DaysOfWeek.Count == 0))
            {
                return (false, "Với mẫu Weekly, bạn phải chọn ít nhất một ngày trong tuần.");
            }

            // Validate days of week are in correct range (2-8 for Vietnamese format)
            if (request.DaysOfWeek != null && request.DaysOfWeek.Any(d => d < 2 || d > 8))
            {
                return (false, "Ngày trong tuần phải từ 2 (Thứ Hai) đến 8 (Chủ Nhật).");
            }

            // Check if date range is too long (optional safety check)
            var daysDifference = request.EndDate.DayNumber - request.StartDate.DayNumber;
            if (daysDifference > 365)
            {
                return (false, "Khoảng thời gian đặt không được vượt quá 1 năm.");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Gets a human-readable description of the recurrence pattern
        /// </summary>
        public static string GetRecurrenceDescription(BookingRecurringRequest request)
        {
            return request.Pattern switch
            {
                RecurrencePattern.Daily => request.Interval == 1 
                    ? "Hàng ngày" 
                    : $"Mỗi {request.Interval} ngày",
                
                RecurrencePattern.Weekly => request.Interval == 1
                    ? $"Hàng tuần vào {GetDayNames(request.DaysOfWeek)}"
                    : $"Mỗi {request.Interval} tuần vào {GetDayNames(request.DaysOfWeek)}",
                
                RecurrencePattern.Weekdays => "Các ngày trong tuần (Thứ 2 - Thứ 6)",
                
                RecurrencePattern.Weekends => "Cuối tuần (Thứ 7 - Chủ Nhật)",
                
                RecurrencePattern.Monthly => request.Interval == 1
                    ? "Hàng tháng"
                    : $"Mỗi {request.Interval} tháng",
                
                RecurrencePattern.Custom => $"Tùy chỉnh: {GetDayNames(request.DaysOfWeek)}",
                
                _ => "Không xác định"
            };
        }

        /// <summary>
        /// Converts day numbers to Vietnamese day names
        /// </summary>
        private static string GetDayNames(List<int> daysOfWeek)
        {
            if (daysOfWeek == null || daysOfWeek.Count == 0)
                return "không có";

            var dayNames = daysOfWeek.Select(d => d switch
            {
                2 => "Thứ Hai",
                3 => "Thứ Ba",
                4 => "Thứ Tư",
                5 => "Thứ Năm",
                6 => "Thứ Sáu",
                7 => "Thứ Bảy",
                8 => "Chủ Nhật",
                _ => "?"
            }).ToList();

            return string.Join(", ", dayNames);
        }

        /// <summary>
        /// Gets the estimated number of bookings that will be created
        /// </summary>
        public static int GetEstimatedBookingCount(BookingRecurringRequest request)
        {
            var dates = GenerateRecurringDates(request);
            return dates.Count;
        }
    }
}
