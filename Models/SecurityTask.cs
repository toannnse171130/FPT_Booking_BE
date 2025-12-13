using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPT_Booking_BE.Models
{
    public class SecurityTask
    {
        [Key]
        public int TaskId { get; set; }

        public string Title { get; set; } = null!; 
        public string? Description { get; set; }   

        public string Status { get; set; } = "Pending";

        public string Priority { get; set; } = "Normal";

        public int? AssignedToUserId { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }

        public string? ReportNote { get; set; }
        public int CreatedBy { get; set; }
    }
}