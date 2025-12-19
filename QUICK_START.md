# Quick Start Guide - Semester & Booking Features

## 🚀 Quick Setup

### 1. Run Migrations (if not already done)
```bash
dotnet ef database update
```

### 2. Seed Initial Data
```bash
# Seed everything (semesters + test bookings + conflicts)
POST http://localhost:5000/api/seed/all
```

## 📋 API Quick Reference

### Semester Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/semesters` | Get all semesters |
| GET | `/api/semesters/current` | Get current active semester |
| GET | `/api/semesters/{id}` | Get semester by ID |
| POST | `/api/semesters` | Create new semester |
| PUT | `/api/semesters/{id}` | Update semester |
| DELETE | `/api/semesters/{id}` | Delete semester |

### Booking Endpoints (Updated)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/bookings` | Create booking (✨ now validates semester) |
| POST | `/api/bookings/recurring` | Create recurring (✨ now validates semester) |
| POST | `/api/bookings/check-recurring-conflicts` | Check all conflicts |

### Seed Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/seed/test-bookings` | Seed diverse test scenarios |
| POST | `/api/seed/conflict-scenarios` | Seed priority conflicts |
| POST | `/api/seed/all` | Seed everything |

## 🧪 Testing Scenarios

### Create a Semester
```http
POST /api/semesters
Content-Type: application/json

{
  "name": "Spring 2025",
  "startDate": "2025-01-01",
  "endDate": "2025-05-31",
  "isActive": true
}
```

### Test Booking Within Semester ✅
```http
POST /api/bookings
Content-Type: application/json

{
  "facilityId": 1,
  "bookingDate": "2025-03-15",
  "slotId": 1,
  "purpose": "Valid booking in semester"
}
```

### Test Booking Outside Semester ❌
```http
POST /api/bookings
Content-Type: application/json

{
  "facilityId": 1,
  "bookingDate": "2025-07-15",
  "slotId": 1,
  "purpose": "Invalid - outside semester"
}
```
**Response:** `"Ngày đặt phòng không nằm trong học kỳ nào. Vui lòng chọn ngày khác."`

### Check Recurring Conflicts
```http
POST /api/bookings/check-recurring-conflicts
Content-Type: application/json

{
  "facilityId": 1,
  "slotId": 1,
  "startDate": "2025-01-20",
  "endDate": "2025-05-15",
  "recurrenceType": "weekly",
  "daysOfWeek": [1, 3, 5],
  "autoFindAlternative": true,
  "skipConflicts": true,
  "purpose": "Weekly class"
}
```

**Response includes:**
- Total dates to be booked
- Conflicts per date
- Alternative room suggestions
- Which dates can proceed
- Which dates are blocked

### Create Recurring Booking
```http
POST /api/bookings/recurring
Content-Type: application/json

{
  "facilityId": 1,
  "slotId": 1,
  "startDate": "2025-01-20",
  "endDate": "2025-05-15",
  "recurrenceType": "weekly",
  "daysOfWeek": [1, 3, 5],
  "purpose": "Weekly Programming Class",
  "autoFindAlternative": true,
  "skipConflicts": true
}
```

## 🔍 Test Data Scenarios

After running `POST /api/seed/all`, you'll have:

### Bookings Created:
1. ✏️ **Pending student booking** (awaiting approval)
2. ✅ **Approved lecturer booking**
3. ⚠️ **Conflict scenario** - same room/slot/date by different users
4. 🔄 **Recurring booking group** - 4 weekly bookings
5. ❌ **Cancelled booking**
6. 🚫 **Rejected booking** with reason
7. 👑 **Admin priority booking**
8. 📅 **Past booking** for history

### Semesters Created:
- Fall 2024 (Sep-Dec, inactive)
- Spring 2025 (Jan-May, **active**)
- Summer 2025 (Jun-Aug, inactive)

### Priority Conflicts:
- Student vs Lecturer (Lecturer can override)
- Lecturer vs Admin (Admin can override)
- Multiple users wanting same slot

## ✨ Key Features

### Semester Validation
- ✅ All bookings must be within a semester
- ✅ Recurring bookings check each date
- ✅ Dates outside semesters are skipped or rejected
- ✅ Clear error messages

### Conflict Detection
- ✅ Lists all conflicts for recurring bookings
- ✅ Shows alternative rooms if available
- ✅ Indicates priority levels
- ✅ Shows which bookings can override

### Role Priority System
| Role | Priority Level | Can Override |
|------|----------------|--------------|
| Admin | 3 | Lecturer, Student |
| Lecturer | 2 | Student |
| Student | 1 | None |
| Security | 0 | None |

## 📝 Notes

- Always create semesters before testing bookings
- Use `/api/semesters/current` to verify active semester
- Use conflict check endpoint before creating recurring bookings
- Seed data is idempotent - won't duplicate if run multiple times
- All validation happens server-side automatically
