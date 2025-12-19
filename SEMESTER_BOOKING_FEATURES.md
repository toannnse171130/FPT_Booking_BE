# Semester & Booking Enhancements

## Overview
This update adds comprehensive semester management and validation to the booking system, along with database seeding utilities for testing.

## New Features

### 1. Semester CRUD Operations

#### Endpoints
- `GET /api/semesters` - Get all semesters
- `GET /api/semesters/{id}` - Get semester by ID
- `GET /api/semesters/current` - Get current active semester
- `POST /api/semesters` - Create new semester
- `PUT /api/semesters/{id}` - Update semester
- `DELETE /api/semesters/{id}` - Delete semester

#### Semester Model
```json
{
  "semesterId": 1,
  "name": "Học kỳ Spring 2025",
  "startDate": "2025-01-01",
  "endDate": "2025-05-31",
  "isActive": true
}
```

#### Example: Create Semester
```bash
POST /api/semesters
Content-Type: application/json

{
  "name": "Học kỳ Fall 2025",
  "startDate": "2025-09-01",
  "endDate": "2025-12-31",
  "isActive": false
}
```

### 2. Booking Validation with Semester

#### Individual Booking
Now validates that booking date falls within an active semester:
- Returns error if booking date is not in any semester
- Prevents booking outside academic periods

```
Error: "Ngày đặt phòng không nằm trong học kỳ nào. Vui lòng chọn ngày khác."
```

#### Recurring Booking
Each generated booking date is checked against semesters:
- Dates outside semesters are skipped (if `skipConflicts` is true)
- Or entire booking fails (if `skipConflicts` is false)
- Detailed logs show which dates failed due to semester issues

### 3. Recurring Booking Conflict Check

The `CheckRecurringConflicts` method already provides comprehensive conflict information:
- Lists all booking dates to be created
- Shows conflicts for each date
- Provides alternative room suggestions
- Indicates which bookings can proceed

#### Example Response
```json
{
  "success": true,
  "totalDates": 12,
  "canProceedCount": 10,
  "blockedCount": 2,
  "conflictCount": 5,
  "message": "Kiểm tra 12 ngày: 10 có thể đặt, 2 bị chặn",
  "conflicts": [
    {
      "bookingDate": "2025-01-20",
      "dayOfWeek": "Thứ Hai",
      "hasConflict": true,
      "canProceed": true,
      "message": "Phòng gốc bận, có thể chuyển sang Lab 302",
      "alternativeFacilityId": "5",
      "alternativeFacilityName": "Lab 302",
      "conflictingBooking": {
        "bookingId": 123,
        "userName": "Nguyễn Văn A",
        "userRole": "Student",
        "facilityName": "Lab 301",
        "canOverride": false
      }
    }
  ]
}
```

### 4. Database Seeding

Three seeding endpoints for testing:

#### Seed Test Bookings
```bash
POST /api/seed/test-bookings
```

Creates diverse test scenarios:
- Normal pending bookings
- Approved lecturer bookings
- Conflict scenarios (same room, slot, date)
- Recurring booking groups
- Cancelled bookings
- Rejected bookings with reasons
- Admin override scenarios
- Past bookings for history

#### Seed Conflict Scenarios
```bash
POST /api/seed/conflict-scenarios
```

Creates priority-based conflict scenarios:
- Student books first (lower priority)
- Lecturer wants same slot (can override)
- Admin wants same slot (highest priority)

#### Seed All
```bash
POST /api/seed/all
```

Runs both test-bookings and conflict-scenarios.

### 5. Automatic Semesters Seeding

When running test-bookings seed, semesters are automatically created:
- Fall 2024 (Sep 1 - Dec 31, inactive)
- Spring 2025 (Jan 1 - May 31, active)
- Summer 2025 (Jun 1 - Aug 31, inactive)

## Implementation Details

### Files Created
- `Repositories/ISemesterRepository.cs`
- `Repositories/SemesterRepository.cs`
- `Services/ISemesterService.cs`
- `Services/SemesterService.cs`
- `Controllers/SemestersController.cs`
- `Controllers/SeedController.cs`
- `DTOs/SemesterDto.cs`
- `Utils/DatabaseSeeder.cs`

### Files Modified
- `Program.cs` - Added DI for Semester services
- `Services/BookingService.cs` - Added semester validation
- `Models/FptFacilityBookingContext.cs` - Added Semester DbSet and entity configuration

## Testing Workflow

1. **Create Semesters**
```bash
POST /api/semesters
{
  "name": "Học kỳ Spring 2025",
  "startDate": "2025-01-01",
  "endDate": "2025-05-31",
  "isActive": true
}
```

2. **Verify Current Semester**
```bash
GET /api/semesters/current
```

3. **Seed Test Data**
```bash
POST /api/seed/all
```

4. **Test Individual Booking**
```bash
POST /api/bookings
{
  "facilityId": 1,
  "bookingDate": "2025-01-20",  // Must be in semester
  "slotId": 1,
  "purpose": "Test booking"
}
```

5. **Check Recurring Conflicts**
```bash
POST /api/bookings/check-recurring-conflicts
{
  "facilityId": 1,
  "slotId": 1,
  "startDate": "2025-01-20",
  "endDate": "2025-05-15",
  "recurrenceType": "weekly",
  "daysOfWeek": [1, 3, 5],  // Mon, Wed, Fri
  "autoFindAlternative": true,
  "skipConflicts": true
}
```

6. **Create Recurring Booking**
```bash
POST /api/bookings/recurring
{
  "facilityId": 1,
  "slotId": 1,
  "startDate": "2025-01-20",
  "endDate": "2025-05-15",
  "recurrenceType": "weekly",
  "daysOfWeek": [1, 3, 5],
  "purpose": "Weekly class",
  "autoFindAlternative": true,
  "skipConflicts": true
}
```

## Notes

- All bookings must now fall within a defined semester
- Recurring bookings will skip dates outside semesters if `skipConflicts` is true
- The conflict checking already lists all conflicts comprehensively
- Database seeder provides realistic test scenarios including edge cases
- Semester validation prevents bookings during breaks or invalid periods
