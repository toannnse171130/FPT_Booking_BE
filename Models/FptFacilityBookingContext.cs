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

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<SecurityTask> SecurityTasks { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=FPT_FacilityBooking;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.AssetId).HasName("PK__Assets__4349237278456401");

            entity.Property(e => e.AssetId).HasColumnName("AssetID");
            entity.Property(e => e.AssetName).HasMaxLength(100);
            entity.Property(e => e.AssetType).HasMaxLength(50);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951ACD4710B574");

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
                .HasConstraintName("FK__Bookings__Approv__60A75C0F");

            entity.HasOne(d => d.Facility).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__Facili__5FB337D6");

            entity.HasOne(d => d.Slot).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__SlotID__619B8048");

            entity.HasOne(d => d.User).WithMany(p => p.BookingUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__UserID__5EBF139D");
        });

        modelBuilder.Entity<Campus>(entity =>
        {
            entity.HasKey(e => e.CampusId).HasName("PK__Campuses__FD598D361D8720C8");

            entity.Property(e => e.CampusId).HasColumnName("CampusID");
            entity.Property(e => e.CampusName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.FacilityId).HasName("PK__Faciliti__5FB08B9427C4EE6C");

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
                .HasConstraintName("FK__Facilitie__Campu__4D94879B");

            entity.HasOne(d => d.Type).WithMany(p => p.Facilities)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Facilitie__TypeI__4E88ABD4");
        });

        modelBuilder.Entity<FacilityAsset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Facility__3214EC27D389F41C");

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
                .HasConstraintName("FK__FacilityA__Asset__5441852A");

            entity.HasOne(d => d.Facility).WithMany(p => p.FacilityAssets)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FacilityA__Facil__534D60F1");
        });

        modelBuilder.Entity<FacilityType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Facility__516F0395D348F4A6");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.RequiresApproval).HasDefaultValue(false);
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32722FA8FA");

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
                .HasConstraintName("FK__Notificat__UserI__6EF57B66");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48E50E81E2C0");

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
                .HasConstraintName("FK__Reports__Booking__693CA210");

            entity.HasOne(d => d.Facility).WithMany(p => p.Reports)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__Reports__Facilit__68487DD7");

            entity.HasOne(d => d.User).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reports__UserID__6754599E");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A991894F7");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160EE9C8B2C").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__Slots__0A124A4F0CA9A8A2");

            entity.Property(e => e.SlotId).HasColumnName("SlotID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SlotName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC6D912594");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534E6A516F4").IsUnique();

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
                .HasConstraintName("FK__Users__RoleID__47DBAE45");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
