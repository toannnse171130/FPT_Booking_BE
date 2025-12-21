using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Utils
{
    public static class DatabaseSeeder
    {
        public static async Task SeedTestData(FptFacilityBookingContext context)
        {
            // Check if data already exists
            if (await context.Bookings.AnyAsync())
            {
                Console.WriteLine("Database already contains booking data. Skipping seed.");
                return;
            }

            Console.WriteLine("Starting database seeding...");

            // Create semesters if not exist
            if (!await context.Semesters.AnyAsync())
            {
                var semesters = new List<Semester>
                {
                    new Semester
                    {
                        Name = "Học kỳ Fall 2024",
                        StartDate = new DateOnly(2024, 9, 1),
                        EndDate = new DateOnly(2024, 12, 31),
                        IsActive = false
                    },
                    new Semester
                    {
                        Name = "Học kỳ Spring 2025",
                        StartDate = new DateOnly(2025, 1, 1),
                        EndDate = new DateOnly(2025, 5, 31),
                        IsActive = true
                    },
                    new Semester
                    {
                        Name = "Học kỳ Summer 2025",
                        StartDate = new DateOnly(2025, 6, 1),
                        EndDate = new DateOnly(2025, 8, 31),
                        IsActive = false
                    }
                };
                await context.Semesters.AddRangeAsync(semesters);
                await context.SaveChangesAsync();
                Console.WriteLine("✓ Seeded Semesters");
            }

            // Get existing users for bookings
            var students = await context.Users.Where(u => u.RoleId == 1).Take(3).ToListAsync();
            var lecturers = await context.Users.Where(u => u.RoleId == 2).Take(2).ToListAsync();
            var admins = await context.Users.Where(u => u.RoleId == 3).Take(1).ToListAsync();

            if (!students.Any() || !lecturers.Any())
            {
                Console.WriteLine("⚠ Not enough users to seed bookings. Please create users first.");
                return;
            }

            // Get existing facilities and slots
            var facilities = await context.Facilities.Take(5).ToListAsync();
            var slots = await context.Slots.Take(4).ToListAsync();

            if (!facilities.Any() || !slots.Any())
            {
                Console.WriteLine("⚠ Not enough facilities or slots to seed bookings.");
                return;
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            var bookings = new List<Booking>();

            // Scenario 1: Normal individual booking by student (Pending)
            bookings.Add(new Booking
            {
                UserId = students[0].UserId,
                FacilityId = facilities[0].FacilityId,
                BookingDate = today.AddDays(2),
                SlotId = slots[0].SlotId,
                Purpose = "Học nhóm môn SWP",
                Status = "Pending",
                BookingType = "Individual",
                PriorityLevel = "Low",
                CreatedAt = DateTime.Now
            });

            // Scenario 2: Approved booking by lecturer
            bookings.Add(new Booking
            {
                UserId = lecturers[0].UserId,
                FacilityId = facilities[1].FacilityId,
                BookingDate = today.AddDays(3),
                SlotId = slots[1].SlotId,
                Purpose = "Dạy lớp SE1234",
                Status = "Approved",
                BookingType = "Individual",
                PriorityLevel = "Medium",
                CreatedAt = DateTime.Now
            });

            // Scenario 3: Conflict scenario - Same room, same slot, same day
            bookings.Add(new Booking
            {
                UserId = students[1].UserId,
                FacilityId = facilities[2].FacilityId,
                BookingDate = today.AddDays(5),
                SlotId = slots[2].SlotId,
                Purpose = "Thuyết trình đồ án",
                Status = "Approved",
                BookingType = "Individual",
                PriorityLevel = "Low",
                CreatedAt = DateTime.Now
            });

            // Another booking on same facility, slot, day to create conflict
            bookings.Add(new Booking
            {
                UserId = lecturers[1].UserId,
                FacilityId = facilities[2].FacilityId,
                BookingDate = today.AddDays(5),
                SlotId = slots[2].SlotId,
                Purpose = "Họp khoa",
                Status = "Pending",
                BookingType = "Individual",
                PriorityLevel = "High",
                CreatedAt = DateTime.Now
            });

            // Scenario 4: Recurring booking group
            var recurrenceId = Guid.NewGuid().ToString();
            for (int i = 0; i < 4; i++)
            {
                bookings.Add(new Booking
                {
                    UserId = students[2].UserId,
                    FacilityId = facilities[3].FacilityId,
                    BookingDate = today.AddDays(7 + (i * 7)), // Weekly for 4 weeks
                    SlotId = slots[0].SlotId,
                    Purpose = "Lớp học định kỳ - Thứ 2 hàng tuần",
                    Status = i == 0 ? "Approved" : "Pending",
                    BookingType = "Group",
                    RecurrenceGroupId = recurrenceId,
                    PriorityLevel = "Low",
                    CreatedAt = DateTime.Now
                });
            }

            // Scenario 5: Cancelled booking
            bookings.Add(new Booking
            {
                UserId = students[0].UserId,
                FacilityId = facilities[4].FacilityId,
                BookingDate = today.AddDays(1),
                SlotId = slots[3].SlotId,
                Purpose = "Đã hủy do thay đổi kế hoạch",
                Status = "Cancelled",
                BookingType = "Individual",
                PriorityLevel = "Low",
                CreatedAt = DateTime.Now.AddDays(-1)
            });

            // Scenario 6: Rejected booking
            bookings.Add(new Booking
            {
                UserId = students[1].UserId,
                FacilityId = facilities[0].FacilityId,
                BookingDate = today.AddDays(4),
                SlotId = slots[1].SlotId,
                Purpose = "Tổ chức sự kiện",
                Status = "Rejected",
                RejectionReason = "Phòng đã được dành cho mục đích khác",
                BookingType = "Individual",
                PriorityLevel = "Low",
                CreatedAt = DateTime.Now.AddDays(-2)
            });

            // Scenario 7: Admin override scenario - high priority approved
            if (admins.Any())
            {
                bookings.Add(new Booking
                {
                    UserId = admins[0].UserId,
                    FacilityId = facilities[1].FacilityId,
                    BookingDate = today.AddDays(6),
                    SlotId = slots[2].SlotId,
                    Purpose = "Sự kiện nhà trường - Ưu tiên cao",
                    Status = "Approved",
                    BookingType = "Individual",
                    PriorityLevel = "High",
                    CreatedAt = DateTime.Now
                });
            }

            // Scenario 8: Past booking (for history)
            bookings.Add(new Booking
            {
                UserId = lecturers[0].UserId,
                FacilityId = facilities[0].FacilityId,
                BookingDate = today.AddDays(-5),
                SlotId = slots[0].SlotId,
                Purpose = "Buổi học đã qua",
                Status = "Approved",
                BookingType = "Individual",
                PriorityLevel = "Medium",
                CreatedAt = DateTime.Now.AddDays(-10)
            });

            await context.Bookings.AddRangeAsync(bookings);
            await context.SaveChangesAsync();

            Console.WriteLine($"✓ Seeded {bookings.Count} test bookings");
            Console.WriteLine("Database seeding completed successfully!");
        }

        public static async Task SeedConflictScenarios(FptFacilityBookingContext context)
        {
            Console.WriteLine("Seeding conflict scenarios...");

            var student = await context.Users.Where(u => u.RoleId == 1).FirstOrDefaultAsync();
            var lecturer = await context.Users.Where(u => u.RoleId == 2).FirstOrDefaultAsync();
            var admin = await context.Users.Where(u => u.RoleId == 3).FirstOrDefaultAsync();

            if (student == null || lecturer == null)
            {
                Console.WriteLine("⚠ Not enough users for conflict scenarios.");
                return;
            }

            var facility = await context.Facilities.FirstOrDefaultAsync();
            var slot = await context.Slots.FirstOrDefaultAsync();

            if (facility == null || slot == null)
            {
                Console.WriteLine("⚠ No facility or slot available.");
                return;
            }

            var conflictDate = DateOnly.FromDateTime(DateTime.Now).AddDays(10);

            var conflictBookings = new List<Booking>
            {
                // Student books first
                new Booking
                {
                    UserId = student.UserId,
                    FacilityId = facility.FacilityId,
                    BookingDate = conflictDate,
                    SlotId = slot.SlotId,
                    Purpose = "Student booking - will be overridden",
                    Status = "Approved",
                    BookingType = "Individual",
                    PriorityLevel = "Low",
                    CreatedAt = DateTime.Now
                },
                // Lecturer wants same slot - can override
                new Booking
                {
                    UserId = lecturer.UserId,
                    FacilityId = facility.FacilityId,
                    BookingDate = conflictDate,
                    SlotId = slot.SlotId,
                    Purpose = "Lecturer booking - higher priority",
                    Status = "Pending",
                    BookingType = "Individual",
                    PriorityLevel = "Medium",
                    CreatedAt = DateTime.Now
                }
            };

            if (admin != null)
            {
                // Admin wants same slot - highest priority
                conflictBookings.Add(new Booking
                {
                    UserId = admin.UserId,
                    FacilityId = facility.FacilityId,
                    BookingDate = conflictDate,
                    SlotId = slot.SlotId,
                    Purpose = "Admin booking - highest priority",
                    Status = "Pending",
                    BookingType = "Individual",
                    PriorityLevel = "High",
                    CreatedAt = DateTime.Now
                });
            }

            await context.Bookings.AddRangeAsync(conflictBookings);
            await context.SaveChangesAsync();

            Console.WriteLine($"✓ Seeded {conflictBookings.Count} conflict scenario bookings");
        }
    }
}
