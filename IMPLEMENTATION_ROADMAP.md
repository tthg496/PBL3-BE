# 🚀 IMPLEMENTATION ROADMAP - GIAI ĐOẠN 4

**Status:** Ready for Implementation  
**Total Features:** 87+ endpoints  
**Estimated Timeline:** 2-3 weeks (sprint-based)

---

## 📋 IMPLEMENTATION PHASES

### Phase 1: Core Authentication & DTOs (Days 1-2)
**Focus:** Foundation - Auth system + All DTOs needed

#### 1.1 Create All Required DTOs
```
Priority: CRITICAL
Time: 4-5 hours

Authentication DTOs:
☐ RegisterDto
☐ LoginDto
☐ VerifyOtpDto
☐ ChangePasswordDto
☐ ForgotPasswordDto
☐ ResetPasswordDto
☐ LoginResponseDto
☐ RefreshTokenDto

Customer DTOs:
☐ CustomerProfileDto
☐ CreateCustomerDto
☐ UpdateCustomerDto

Manager DTOs:
☐ ManagerProfileDto
☐ UpdateManagerDto
☐ ActivityLogDto

Employee DTOs:
☐ EmployeeProfileDto
☐ CreateEmployeeDto
☐ UpdateEmployeeDto
☐ EmployeeStatisticsDto
☐ EmployeeInviteDto
```

#### 1.2 Implement AuthService Methods
```
Priority: CRITICAL
Time: 3-4 hours

Methods to implement:
☐ RegisterAsync(RegisterDto)
☐ VerifyOtpAsync(VerifyOtpDto)
☐ LoginAsync(LoginDto)
☐ GenerateJwtToken()
☐ GenerateRefreshToken()
☐ ChangePasswordAsync()
☐ ForgotPasswordAsync()
☐ ResetPasswordAsync()
☐ RefreshTokenAsync()

Features:
• Email/OTP validation
• Password hashing (bcrypt)
• JWT token generation (1 hour)
• Refresh token (7 days)
• Token blacklisting
```

#### 1.3 Add JWT Authentication Middleware
```
Priority: CRITICAL
Time: 2-3 hours

Tasks:
☐ Create JwtTokenProvider
☐ Implement JWT token generation
☐ Add token validation
☐ Create custom [Authorize] attribute
☐ Add exception handling for auth errors
☐ Configure in Program.cs

Security:
• Asymmetric encryption
• Token expiration
• Token refresh strategy
```

**Deliverables:**
- ✅ All DTOs created
- ✅ AuthService fully functional
- ✅ JWT authentication working
- ✅ Register/Login/OTP flow complete

---

### Phase 2: Customer Features (Days 3-5)
**Focus:** Customer portal - Account + Reservations + Tickets

#### 2.1 Account Management
```
Priority: HIGH
Time: 2-3 hours

Endpoints to implement:
☐ GET /api/auth/me
☐ PUT /api/customers/{id}
☐ POST /api/auth/change-password

Service methods:
☐ GetCustomerProfileAsync()
☐ UpdateCustomerAsync()
☐ ChangePasswordAsync()

DTOs:
☐ CustomerProfileDto
☐ UpdateCustomerDto
```

#### 2.2 Reservations Management
```
Priority: HIGH
Time: 4-5 hours

Endpoints to implement:
☐ GET /api/reservations
☐ GET /api/reservations/{id}
☐ POST /api/reservations
☐ DELETE /api/reservations/{id}
☐ GET /api/reservations/available-slots

Service methods:
☐ GetReservationsAsync()
☐ GetReservationByIdAsync()
☐ CreateReservationAsync()
☐ CancelReservationAsync()
☐ GetAvailableSlotsAsync()

DTOs:
☐ ReservationDto
☐ CreateReservationDto
☐ UpdateReservationDto
☐ AvailableSlotDto

Features:
• Slot availability check
• Auto-assign best slot
• QR code generation
• Email confirmation
```

#### 2.3 Tickets Management
```
Priority: HIGH
Time: 3-4 hours

Endpoints to implement:
☐ GET /api/tickets
☐ GET /api/tickets/{id}
☐ GET /api/tickets/{id}/payment-history

Service methods:
☐ GetTicketsAsync()
☐ GetTicketByIdAsync()
☐ GetPaymentHistoryAsync()

DTOs:
☐ TicketDto
☐ PaymentDto
☐ TicketSummaryDto
```

#### 2.4 Monthly Tickets
```
Priority: MEDIUM
Time: 3-4 hours

Endpoints to implement:
☐ GET /api/monthly-tickets/customer/{id}
☐ GET /api/monthly-tickets/{id}
☐ GET /api/pricing/monthly
☐ POST /api/monthly-tickets
☐ POST /api/monthly-tickets/{id}/renew

Service methods:
☐ GetMonthlyTicketsAsync()
☐ GetMonthlyTicketByIdAsync()
☐ GetMonthlyPricingAsync()
☐ PurchaseMonthlyTicketAsync()
☐ RenewMonthlyTicketAsync()

DTOs:
☐ MonthlyTicketDto
☐ CreateMonthlyTicketDto
☐ PricingPackageDto
```

**Deliverables:**
- ✅ Customer can register and login
- ✅ Manage profile and reservations
- ✅ View tickets and monthly subscriptions
- ✅ Purchase and renew monthly tickets

---

### Phase 3: Employee Features (Days 6-8)
**Focus:** Employee operations - Check-in/out + Customers + Reports

#### 3.1 Employee Account & Setup
```
Priority: HIGH
Time: 1-2 hours

Endpoints to implement:
☐ GET /api/auth/me
☐ PUT /api/employees/{id}
☐ POST /api/auth/change-password

Service methods:
☐ GetEmployeeProfileAsync()
☐ UpdateEmployeeAsync()
```

#### 3.2 Ticket Operations (Check-in/out)
```
Priority: CRITICAL
Time: 4-5 hours

Endpoints to implement:
☐ POST /api/tickets/checkin/confirm
☐ POST /api/tickets/checkout/confirm
☐ POST /api/tickets (create manual)
☐ GET /api/employees/tickets
☐ GET /api/employees/tickets/search

Service methods:
☐ CheckInAsync()
☐ CheckOutAsync()
☐ CreateTicketAsync()
☐ GetEmployeeTicketsAsync()

DTOs:
☐ CheckInDto
☐ CheckOutDto
☐ CreateTicketDto

Features:
• Parking slot status update
• Fee calculation
• Payment processing
• QR code scanning
```

#### 3.3 Customer Management (Employee View)
```
Priority: MEDIUM
Time: 2-3 hours

Endpoints to implement:
☐ GET /api/employees/customers
☐ GET /api/employees/customers/search

Service methods:
☐ GetCustomersAsync()
☐ SearchCustomersAsync()

DTOs:
☐ CustomerInfoDto
```

#### 3.4 Parking Slots (Employee View)
```
Priority: MEDIUM
Time: 2-3 hours

Endpoints to implement:
☐ GET /api/employees/parking-slots
☐ GET /api/employees/parking-slots/{id}

Service methods:
☐ GetEmployeeSlotsAsync()
☐ GetSlotDetailAsync()
```

#### 3.5 Employee Dashboard & Reports
```
Priority: MEDIUM
Time: 3-4 hours

Endpoints to implement:
☐ GET /api/reports/employee/{id}/dashboard
☐ GET /api/reports/employee/{id}/attendance
☐ GET /api/reports/employee/{id}/revenue

Service methods:
☐ GetEmployeeDashboardAsync()
☐ GetAttendanceReportAsync()
☐ GetRevenueReportAsync()

DTOs:
☐ EmployeeDashboardDto
☐ AttendanceReportDto
☐ RevenueReportDto
```

**Deliverables:**
- ✅ Employee can login
- ✅ Check-in and check-out vehicles
- ✅ Process payments
- ✅ View personal dashboard and reports

---

### Phase 4: Manager Features (Days 9-12)
**Focus:** Management & Analytics - Employees + Slots + Reports

#### 4.1 Employee Management
```
Priority: HIGH
Time: 4-5 hours

Endpoints to implement:
☐ GET /api/employees
☐ GET /api/employees/{id}/detail
☐ GET /api/employees/search
☐ POST /api/employees/invite
☐ PUT /api/employees/{id}
☐ DELETE /api/employees/{id}

Service methods:
☐ GetEmployeesAsync()
☐ GetEmployeeDetailAsync()
☐ SearchEmployeesAsync()
☐ InviteEmployeeAsync()
☐ UpdateEmployeeAsync()
☐ DeleteEmployeeAsync()

DTOs:
☐ EmployeeListDto
☐ EmployeeDetailDto
☐ EmployeeStatisticsDto
☐ InviteEmployeeDto
```

#### 4.2 Ticket Management (Manager View)
```
Priority: HIGH
Time: 3-4 hours

Endpoints to implement:
☐ GET /api/tickets
☐ GET /api/tickets/search
☐ GET /api/pricing
☐ PUT /api/pricing/{id}

Service methods:
☐ GetAllTicketsAsync()
☐ SearchTicketsAsync()
☐ GetPricingAsync()
☐ UpdatePricingAsync()

DTOs:
☐ TicketSummaryDto
☐ PricingDto
☐ UpdatePricingDto
```

#### 4.3 Parking Slots Management
```
Priority: HIGH
Time: 4-5 hours

Endpoints to implement:
☐ GET /api/parking-slots
☐ GET /api/parking-slots/{id}
☐ PATCH /api/parking-slots/{id}/status
☐ GET /api/parking-slots/summary
☐ GET /api/parking-slots/report

Service methods:
☐ GetAllSlotsAsync()
☐ GetSlotDetailAsync()
☐ UpdateSlotStatusAsync()
☐ GetSlotsummaryAsync()
☐ GenerateParkingReportAsync()

DTOs:
☐ ParkingSlotDto
☐ SlotStatusDto
☐ SlotSummaryDto
☐ ParkingReportDto
```

#### 4.4 Dashboard & Analytics
```
Priority: HIGH
Time: 5-6 hours

Endpoints to implement:
☐ GET /api/reports/manager/dashboard
☐ POST /api/reports/manager/revenue
☐ GET /api/reports/manager/customers
☐ GET /api/reports/manager/comprehensive
☐ GET /api/managers/{id}/activity-log

Service methods:
☐ GetManagerDashboardAsync()
☐ GenerateRevenueReportAsync()
☐ GetCustomerReportAsync()
☐ GenerateComprehensiveReportAsync()
☐ GetActivityLogAsync()

DTOs:
☐ ManagerDashboardDto
☐ RevenueReportDto
☐ CustomerReportDto
☐ ComprehensiveReportDto
☐ ActivityLogDto

Features:
• Revenue charts
• Occupancy rate
• Customer statistics
• Employee performance
• Activity tracking
```

#### 4.5 Manager Account
```
Priority: MEDIUM
Time: 1-2 hours

Endpoints to implement:
☐ GET /api/auth/me
☐ PUT /api/managers/{id}
☐ POST /api/auth/change-password

Service methods:
☐ GetManagerProfileAsync()
☐ UpdateManagerAsync()
```

**Deliverables:**
- ✅ Manager can manage employees
- ✅ Monitor parking slots
- ✅ View comprehensive reports
- ✅ Manage pricing and revenue
- ✅ Track activities

---

## 📅 IMPLEMENTATION SCHEDULE

| Phase | Features | Duration | Days |
|-------|----------|----------|------|
| **1** | Auth + DTOs | 9-12 hrs | 1-2 |
| **2** | Customer | 12-15 hrs | 3-5 |
| **3** | Employee | 12-15 hrs | 6-8 |
| **4** | Manager | 18-22 hrs | 9-12 |
| **5** | Testing | 8-10 hrs | 13-14 |
| **6** | Deployment | 2-3 hrs | 15 |
| | **TOTAL** | **~60-75 hrs** | **~15 days** |

---

## 🎯 DAILY BREAKDOWN

### Day 1-2: Auth & DTOs
```
Morning: Create all DTOs
Afternoon: Implement AuthService
Evening: Add JWT middleware
Deliverable: Auth system working
```

### Day 3-5: Customer Features
```
Day 3: Account + Reservations
Day 4: Tickets + Monthly Tickets
Day 5: Testing + Fixes
Deliverable: Full customer portal
```

### Day 6-8: Employee Features
```
Day 6: Account + Check-in/out
Day 7: Customers + Slots + Dashboard
Day 8: Testing + Optimization
Deliverable: Full employee operations
```

### Day 9-12: Manager Features
```
Day 9: Employee Management
Day 10: Ticket + Slot Management
Day 11: Dashboard + Reports
Day 12: Testing + Optimization
Deliverable: Full admin panel
```

### Day 13-15: Testing & Deployment
```
Day 13: Unit tests
Day 14: Integration tests
Day 15: Deployment prep
Deliverable: Production ready
```

---

## 📝 TESTING STRATEGY

### Unit Tests
```
Services:
☐ AuthService (10+ tests)
☐ CustomerService (8+ tests)
☐ EmployeeService (12+ tests)
☐ TicketService (10+ tests)
☐ ReportService (8+ tests)
☐ ParkingSlotService (12+ tests)
☐ ReservationService (10+ tests)
☐ MonthlyTicketService (8+ tests)

Coverage Target: 80%+
```

### Integration Tests
```
API Endpoints:
☐ Auth endpoints (5+ tests)
☐ Customer endpoints (15+ tests)
☐ Employee endpoints (12+ tests)
☐ Manager endpoints (20+ tests)
```

### Manual Testing
```
Scenarios:
☐ Customer registration flow
☐ Reservation & check-in flow
☐ Payment processing
☐ Employee shift management
☐ Manager reports
☐ Error handling
```

---

## 🔧 TOOLS & TECHNOLOGIES

### Development
- **Language:** C#
- **Framework:** ASP.NET Core 8
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Authentication:** JWT

### Testing
- **Unit Testing:** xUnit / NUnit
- **Mocking:** Moq
- **Integration:** Postman / REST Client

### API Documentation
- **Swagger/OpenAPI:** Auto-generated
- **API Documentation:** Postman collection

---

## 📊 SUCCESS CRITERIA

### Phase 1
- ✅ All DTOs created
- ✅ Auth working end-to-end
- ✅ JWT tokens generated
- ✅ 0 build errors

### Phase 2
- ✅ Customer can register/login
- ✅ All customer endpoints working
- ✅ Reservations system functional
- ✅ Monthly tickets working

### Phase 3
- ✅ Employee can login
- ✅ Check-in/out working
- ✅ Payment processing working
- ✅ Personal reports accessible

### Phase 4
- ✅ Employee management working
- ✅ Reports generating correctly
- ✅ Dashboard showing real data
- ✅ All manager features working

### Phase 5
- ✅ 80%+ test coverage
- ✅ All tests passing
- ✅ No critical bugs
- ✅ Performance acceptable

---

## 🚀 NEXT STEPS

1. **Review this roadmap** with your team
2. **Create DTOs first** - They're the foundation
3. **Implement AuthService** - Everything depends on it
4. **Follow the phase schedule** - Keep momentum
5. **Test continuously** - Don't leave for the end

---

## 💡 TIPS FOR SUCCESS

1. **Group related work** - Don't jump between features
2. **Test as you go** - Write tests alongside code
3. **Use existing patterns** - EmployeeService is a good template
4. **Review consolidated structure** - Follow what was already done
5. **Keep DTOs consistent** - Naming, structure, validation
6. **Update documentation** - As you implement
7. **Get feedback early** - Share progress with team

---

## 📈 TRACKING PROGRESS

### Checklist Template
```
Phase 1: Auth
├── DTOs
│   ├── ☐ Authentication DTOs
│   ├── ☐ Customer DTOs
│   ├── ☐ Manager DTOs
│   └── ☐ Employee DTOs
├── Service Methods
│   ├── ☐ Register
│   ├── ☐ Login
│   ├── ☐ OTP Verification
│   └── ☐ Password Management
├── Middleware
│   ├── ☐ JWT Generation
│   ├── ☐ Token Validation
│   └── ☐ Authorization
└── Testing
    ├── ☐ Unit Tests
    └── ☐ Integration Tests
```

---

**Status:** Ready to begin  
**Start Date:** Next sprint  
**Target Completion:** ~15 days  
**Team Size:** 2-3 developers recommended

**Let's build this! 🚀**
