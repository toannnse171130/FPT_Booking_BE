using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Models;

public partial class FptFacilityBookingContext : DbContext
{
    public FptFacilityBookingContext()
    {
    }

    public FptFacilityBookingContext(DbContextOptions<FptFacilityBookingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Campus> Campuses { get; set; }

    public virtual DbSet<Facility> Facilities { get; set; }

    public virtual DbSet<FacilityAsset> FacilityAssets { get; set; }

    public virtual DbSet<FacilityType> FacilityTypes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<RecurrencePattern> RecurrencePatterns { get; set; }

    public virtual DbSet<RecurrencePatternType> RecurrencePatternTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SecurityTask> SecurityTasks { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Fallback connection string for EF Core tools (migrations)
            // In production, connection string is provided via DI in Program.cs
            var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("MyDbConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.AssetId).HasName("PK__Assets__434923723A6A61AA");

            entity.Property(e => e.AssetId).HasColumnName("AssetID");
            entity.Property(e => e.AssetName).HasMaxLength(100);
            entity.Property(e => e.AssetType).HasMaxLength(50);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951ACD0D86927F");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.ApprovedAt).HasColumnType("datetime");
            entity.Property(e => e.ApproverId).HasColumnName("ApproverID");
            entity.Property(e => e.BookingType)
                .HasMaxLength(20)
                .HasDefaultValue("Individual");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.PriorityLevel)
                .HasMaxLength(20)
                .HasDefaultValue("Medium");
            entity.Property(e => e.Purpose).HasMaxLength(255);
            entity.Property(e => e.RecurrenceGroupId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RecurrenceGroupID");
            entity.Property(e => e.SlotId).HasColumnName("SlotID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Approver).WithMany(p => p.BookingApprovers)
                .HasForeignKey(d => d.ApproverId)
                .HasConstraintName("FK__Bookings__Approv__73BA3083");

            entity.HasOne(d => d.Facility).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__Facili__72C60C4A");

            entity.HasOne(d => d.Slot).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__SlotID__74AE54BC");

            entity.HasOne(d => d.RecurrencePattern).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RecurrencePatternId)
                .HasConstraintName("FK_Bookings_RecurrencePattern");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BookingUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_Bookings_UpdatedBy");

            entity.HasOne(d => d.User).WithMany(p => p.BookingUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__UserID__71D1E811");
        });

        modelBuilder.Entity<Campus>(entity =>
        {
            entity.HasKey(e => e.CampusId).HasName("PK__Campuses__FD598D3696DA5EA2");

            entity.Property(e => e.CampusId).HasColumnName("CampusID");
            entity.Property(e => e.CampusName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.FacilityId).HasName("PK__Faciliti__5FB08B9477278C1D");

            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.CampusId).HasColumnName("CampusID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FacilityName).HasMaxLength(100);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Available");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Campus).WithMany(p => p.Facilities)
                .HasForeignKey(d => d.CampusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Facilitie__Campu__60A75C0F");

            entity.HasOne(d => d.Type).WithMany(p => p.Facilities)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Facilitie__TypeI__619B8048");
        });

        modelBuilder.Entity<FacilityAsset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Facility__3214EC27B673F84B");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AssetId).HasColumnName("AssetID");
            entity.Property(e => e.Condition)
                .HasMaxLength(50)
                .HasDefaultValue("Tốt");
            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Asset).WithMany(p => p.FacilityAssets)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FacilityA__Asset__6754599E");

            entity.HasOne(d => d.Facility).WithMany(p => p.FacilityAssets)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FacilityA__Facil__66603565");
        });

        modelBuilder.Entity<FacilityType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Facility__516F039515EF4403");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.RequiresApproval).HasDefaultValue(false);
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E325A705AD8");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasDefaultValue("System");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__02084FDA");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48E52C7C967E");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.ReportType).HasMaxLength(50);
            entity.Property(e => e.ResolvedAt).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Booking).WithMany(p => p.Reports)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Reports__Booking__7C4F7684");

            entity.HasOne(d => d.Facility).WithMany(p => p.Reports)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__Reports__Facilit__7B5B524B");

            entity.HasOne(d => d.User).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reports__UserID__7A672E12");
        });

        modelBuilder.Entity<RecurrencePatternType>(entity =>
        {
            entity.HasKey(e => e.PatternTypeId).HasName("PK__RecurrencePatternType__ID");

            entity.Property(e => e.PatternTypeId).HasColumnName("PatternTypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(255);
        });

        modelBuilder.Entity<RecurrencePattern>(entity =>
        {
            entity.HasKey(e => e.RecurrencePatternId).HasName("PK__Recurren__PATTERN_ID");

            entity.Property(e => e.RecurrencePatternId).HasColumnName("RecurrencePatternID");
            entity.Property(e => e.RecurrenceGroupId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RecurrenceGroupID");
            entity.Property(e => e.PatternTypeId).HasColumnName("PatternTypeID");
            entity.Property(e => e.DaysOfWeek).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            
            entity.HasOne(d => d.CreatedByUser).WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecurrencePattern_CreatedBy");

            entity.HasOne(d => d.PatternType).WithMany(p => p.RecurrencePatterns)
                .HasForeignKey(d => d.PatternTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecurrencePattern_PatternType");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AB5D9A902");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61605F36FFA0").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<SecurityTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Security__7C6949B1C91D2A0E");

            entity.Property(e => e.CompletedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Priority)
                .HasMaxLength(50)
                .HasDefaultValue("Normal");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.AssignedToUser).WithMany(p => p.SecurityTaskAssignedToUsers)
                .HasForeignKey(d => d.AssignedToUserId)
                .HasConstraintName("FK_SecurityTasks_AssignedToUser");

            entity.HasOne(d => d.Booking).WithMany(p => p.SecurityTasks)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SecurityTasks_Booking");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SecurityTaskCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SecurityTasks_CreatedBy");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.SemesterId).HasName("PK__Semester__4BD03CA5A2A3E7B1");

            entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.IsActive).HasDefaultValue(false);
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__Slots__0A124A4F26F6E8CC");

            entity.Property(e => e.SlotId).HasColumnName("SlotID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SlotName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACBAD4AC53");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105348CD2578C").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleID__5AEE82B9");
        });

        // Seed Data
        SeedData(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "Admin", Description = "Quản trị viên hệ thống" },
            new Role { RoleId = 2, RoleName = "FacilityAdmin", Description = "Người duyệt phòng" },
            new Role { RoleId = 3, RoleName = "Student", Description = "Sinh viên" },
            new Role { RoleId = 4, RoleName = "Lecturer", Description = "Giảng viên" },
            new Role { RoleId = 5, RoleName = "Manager", Description = "Quản lý thiết bị" },
            new Role { RoleId = 6, RoleName = "Security", Description = "Bảo vệ" }
        );

        // Seed Campuses
        modelBuilder.Entity<Campus>().HasData(
            new Campus { CampusId = 1, CampusName = "FU FPT High Tech Park", Address = "Lô E2a-7, Đường D1, Khu Công Nghệ Cao, Tp. Thủ Đức", IsActive = true },
            new Campus { CampusId = 2, CampusName = "NVH Thanh Nien", Address = "04 Phạm Ngọc Thạch, Bến Nghé, Quận 1", IsActive = true }
        );

        // Seed Facility Types
        modelBuilder.Entity<FacilityType>().HasData(
            new FacilityType { TypeId = 1, TypeName = "Phòng học", RequiresApproval = false, Description = "Phòng học thông thường" },
            new FacilityType { TypeId = 2, TypeName = "Phòng Lab", RequiresApproval = true, Description = "Phòng thí nghiệm" },
            new FacilityType { TypeId = 3, TypeName = "Hội trường", RequiresApproval = true, Description = "Hội trường sự kiện" },
            new FacilityType { TypeId = 4, TypeName = "Sân thể thao", RequiresApproval = false, Description = "Sân thể thao" }
        );

        // Seed Recurrence Pattern Types
        modelBuilder.Entity<RecurrencePatternType>().HasData(
            new RecurrencePatternType { PatternTypeId = 1, TypeName = "Daily", Description = "Repeat every day" },
            new RecurrencePatternType { PatternTypeId = 2, TypeName = "Weekly", Description = "Repeat every week on the same day(s)" },
            new RecurrencePatternType { PatternTypeId = 3, TypeName = "Weekdays", Description = "Repeat on weekdays only (Monday to Friday)" },
            new RecurrencePatternType { PatternTypeId = 4, TypeName = "Weekends", Description = "Repeat on weekends only (Saturday and Sunday)" },
            new RecurrencePatternType { PatternTypeId = 5, TypeName = "Monthly", Description = "Repeat every month on the same date" },
            new RecurrencePatternType { PatternTypeId = 6, TypeName = "Custom", Description = "Custom pattern - specify which days of week" },
            new RecurrencePatternType { PatternTypeId = 7, TypeName = "Semesterly", Description = "Repeat for the entire semester" }
        );

        // Seed Slots (Ca học FPT)
        modelBuilder.Entity<Slot>().HasData(
            new Slot { SlotId = 1, SlotName = "Slot 1", StartTime = new TimeOnly(7, 0), EndTime = new TimeOnly(9, 15), IsActive = true },
            new Slot { SlotId = 2, SlotName = "Slot 2", StartTime = new TimeOnly(9, 30), EndTime = new TimeOnly(11, 45), IsActive = true },
            new Slot { SlotId = 3, SlotName = "Slot 3", StartTime = new TimeOnly(12, 30), EndTime = new TimeOnly(14, 45), IsActive = true },
            new Slot { SlotId = 4, SlotName = "Slot 4", StartTime = new TimeOnly(15, 0), EndTime = new TimeOnly(17, 15), IsActive = true },
            new Slot { SlotId = 5, SlotName = "Slot 5", StartTime = new TimeOnly(17, 30), EndTime = new TimeOnly(19, 45), IsActive = true },
            new Slot { SlotId = 6, SlotName = "Slot 6", StartTime = new TimeOnly(19, 30), EndTime = new TimeOnly(21, 0), IsActive = true }
        );

        // Seed Assets
        modelBuilder.Entity<Asset>().HasData(
            new Asset { AssetId = 1, AssetName = "Máy chiếu Sony 4K", AssetType = "Điện tử", Description = "Máy chiếu chất lượng cao" },
            new Asset { AssetId = 2, AssetName = "PC Gaming ROG", AssetType = "Máy tính", Description = "Máy tính gaming hiệu năng cao" },
            new Asset { AssetId = 3, AssetName = "Loa JBL Hall", AssetType = "Âm thanh", Description = "Hệ thống âm thanh hội trường" },
            new Asset { AssetId = 4, AssetName = "Điều hòa Daikin", AssetType = "Điện lạnh", Description = "Máy điều hòa công suất lớn" }
        );

        // Seed Users (Password: "123456" - plain text for demo)
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                UserId = 1, 
                Email = "admin@fpt.edu.vn", 
                PasswordHash = "123456",
                FullName = "Nguyễn Quản Trị", 
                RoleId = 1, 
                PhoneNumber = "0901234567",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new User 
            { 
                UserId = 2, 
                Email = "admin_room@fpt.edu.vn", 
                PasswordHash = "123456",
                FullName = "Lê Duyệt Phòng", 
                RoleId = 2, 
                PhoneNumber = "0909999888",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new User 
            { 
                UserId = 3, 
                Email = "student@fpt.edu.vn", 
                PasswordHash = "123456",
                FullName = "Trần Sinh Viên", 
                RoleId = 3, 
                PhoneNumber = "0912345678",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new User 
            { 
                UserId = 4, 
                Email = "lecturer@fpt.edu.vn", 
                PasswordHash = "123456",
                FullName = "Phạm Giảng Viên", 
                RoleId = 4, 
                PhoneNumber = "0987654321",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new User 
            { 
                UserId = 5, 
                Email = "security@fpt.edu.vn", 
                PasswordHash = "123456",
                FullName = "Chú Bảo Vệ", 
                RoleId = 6, 
                PhoneNumber = "0900000001",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new User 
            { 
                UserId = 6, 
                Email = "s1", 
                PasswordHash = "1",
                FullName = "Sinh vien 1", 
                RoleId = 3, 
                PhoneNumber = "0900000002",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new User 
            { 
                UserId = 7, 
                Email = "string", 
                PasswordHash = "string",
                FullName = "Nguyễn Quản Admin", 
                RoleId = 1, 
                PhoneNumber = "0901234567",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            }
        );

        // Seed Facilities
        modelBuilder.Entity<Facility>().HasData(
            new Facility 
            { 
                FacilityId = 1, 
                FacilityName = "Lab AI - Room 302", 
                TypeId = 2, 
                CampusId = 1, 
                Capacity = 40, 
                Status = "Available",
                Description = "Phòng Lab AI với 40 PC Gaming",
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new Facility 
            { 
                FacilityId = 2, 
                FacilityName = "Hội trường Beta", 
                TypeId = 3, 
                CampusId = 1, 
                Capacity = 200, 
                Status = "Available",
                Description = "Hội trường lớn cho sự kiện",
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new Facility 
            { 
                FacilityId = 3, 
                FacilityName = "Phòng học  - Room 300", 
                TypeId = 1, 
                CampusId = 1, 
                Capacity = 40, 
                Status = "Available",
                Description = "Phòng học tiêu chuẩn",
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new Facility 
            { 
                FacilityId = 4, 
                FacilityName = "Sân bóng đá mini", 
                TypeId = 4, 
                CampusId = 1, 
                Capacity = 200, 
                Status = "Available",
                Description = "Sân bóng đá mini cho sinh viên",
                CreatedAt = new DateTime(2024, 1, 1)
            }
        );

        // Seed Facility Assets
        modelBuilder.Entity<FacilityAsset>().HasData(
            new FacilityAsset { Id = 1, FacilityId = 1, AssetId = 1, Quantity = 1, Condition = "Tốt" },
            new FacilityAsset { Id = 2, FacilityId = 1, AssetId = 2, Quantity = 40, Condition = "Tốt" },
            new FacilityAsset { Id = 3, FacilityId = 1, AssetId = 4, Quantity = 2, Condition = "Tốt" },
            new FacilityAsset { Id = 4, FacilityId = 2, AssetId = 1, Quantity = 2, Condition = "Tốt" },
            new FacilityAsset { Id = 5, FacilityId = 2, AssetId = 3, Quantity = 1, Condition = "Tốt" }
        );

        // Seed Bookings
        var today = DateOnly.FromDateTime(DateTime.Now);
        var tomorrow = today.AddDays(1);
        
        modelBuilder.Entity<Booking>().HasData(
            new Booking 
            { 
                BookingId = 1, 
                UserId = 3, 
                FacilityId = 1, 
                BookingDate = today, 
                SlotId = 1, 
                Purpose = "Học nhóm Java",
                Status = "Approved",
                PriorityLevel = "Low",
                BookingType = "Individual",
                CreatedAt = new DateTime(2024, 1, 1),
                UpdatedAt = new DateTime(2024, 1, 1)
            },
            new Booking 
            { 
                BookingId = 2, 
                UserId = 4, 
                FacilityId = 2, 
                BookingDate = tomorrow, 
                SlotId = 2, 
                Purpose = "Hội thảo Tech",
                Status = "Pending",
                PriorityLevel = "High",
                BookingType = "Individual",
                CreatedAt = new DateTime(2024, 1, 1),
                UpdatedAt = new DateTime(2024, 1, 1)
            }
        );

        // Seed Reports
        modelBuilder.Entity<Report>().HasData(
            new Report 
            { 
                ReportId = 1, 
                UserId = 3, 
                FacilityId = 1, 
                Title = "PC số 05 bị màn hình xanh",
                Description = "Đang code thì bị tắt máy",
                Status = "Pending",
                ReportType = "Hỏng thiết bị",
                CreatedAt = new DateTime(2024, 1, 1)
            }
        );

        // Seed Notifications
        modelBuilder.Entity<Notification>().HasData(
            new Notification 
            { 
                NotificationId = 1, 
                UserId = 3, 
                Title = "Đặt phòng thành công",
                Message = "Yêu cầu Slot 1 hôm nay đã được duyệt.",
                Type = "Booking",
                IsRead = false,
                CreatedAt = new DateTime(2024, 1, 1)
            }
        );
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
