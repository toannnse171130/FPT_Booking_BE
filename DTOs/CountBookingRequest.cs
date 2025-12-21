using System;

namespace FPT_Booking_BE.DTOs
{
    public class  CountBookingRequest 
    {
        public int? UserId { get; set; }     

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string SortBy { get; set; } = "Newest";
    }

