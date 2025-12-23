using System;
using System.Collections.Generic;

namespace FPT_Booking_BE.Models;

public partial class SecurityTask
{
    public int TaskId { get; set; }

    public string Title { get; set; } = null!;
    public int BookingId { get; set; } 

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public string Priority { get; set; } = null!;
    public string? TaskType { get; set; } = null!;

    public int? AssignedToUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? ReportNote { get; set; }

    public int CreatedBy { get; set; }

    public virtual User? AssignedToUser { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;
}
