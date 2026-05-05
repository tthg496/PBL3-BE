using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParkingManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2026 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSlots",
                columns: table => new
                {
                    SlotId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSlots", x => x.SlotId);
                    table.CheckConstraint("CK_ParkingSlot_Status", "Status IN (N'Trống', N'Đang sử dụng', N'Đã đặt', N'Bảo trì')");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Shift = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehiclePlate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehiclePlate);
                    table.CheckConstraint("CK_Vehicle_Type", "VehicleType IN (N'Xe máy', N'Ô tô nhỏ', N'Ô tô lớn')");
                    table.ForeignKey(
                        name: "FK_Vehicles_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    ManagerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_Managers_Employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyTickets",
                columns: table => new
                {
                    MonthlyTicketId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VehiclePlate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PackageType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalFee = table.Column<decimal>(type: "decimal(10,0)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyTickets", x => x.MonthlyTicketId);
                    table.CheckConstraint("CK_MonthlyTicket_Status", "Status IN (N'Hoạt động', N'Hết hạn', N'Đã hủy')");
                    table.ForeignKey(
                        name: "FK_MonthlyTickets_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyTickets_Vehicles_VehiclePlate",
                        column: x => x.VehiclePlate,
                        principalTable: "Vehicles",
                        principalColumn: "VehiclePlate",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VehiclePlate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SlotId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ExpectedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationId);
                    table.CheckConstraint("CK_Reservation_Status", "Status IN (N'Chờ', N'Đã nhận', N'Hủy', N'Hết hạn')");
                    table.ForeignKey(
                        name: "FK_Reservations_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_ParkingSlots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "ParkingSlots",
                        principalColumn: "SlotId");
                    table.ForeignKey(
                        name: "FK_Reservations_Vehicles_VehiclePlate",
                        column: x => x.VehiclePlate,
                        principalTable: "Vehicles",
                        principalColumn: "VehiclePlate");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VehiclePlate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SlotId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Fee = table.Column<decimal>(type: "decimal(10,0)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.CheckConstraint("CK_Ticket_Status", "Status IN (N'Đang trong bãi', N'Đã ra')");
                    table.ForeignKey(
                        name: "FK_Tickets_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_Tickets_ParkingSlots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "ParkingSlots",
                        principalColumn: "SlotId");
                    table.ForeignKey(
                        name: "FK_Tickets_Vehicles_VehiclePlate",
                        column: x => x.VehiclePlate,
                        principalTable: "Vehicles",
                        principalColumn: "VehiclePlate",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TicketId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MonthlyTicketId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(10,0)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.CheckConstraint("CK_Payment_TicketOrMonthly", "(TicketId IS NOT NULL AND MonthlyTicketId IS NULL) OR (TicketId IS NULL AND MonthlyTicketId IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Payments_MonthlyTickets_MonthlyTicketId",
                        column: x => x.MonthlyTicketId,
                        principalTable: "MonthlyTickets",
                        principalColumn: "MonthlyTicketId");
                    table.ForeignKey(
                        name: "FK_Payments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountId", "CreatedAt", "Email", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { "ACC001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tung.ql@baixetranphu.vn", true, "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMaJnFd1jC9s5sJq1bKx5HY4i6", "Manager", "nguyen.thanh.tung" },
                    { "ACC002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mai.nv@baixetranphu.vn", true, "$2a$12$XpV8cBnqRT5HaK2dLM7OiuZqY1P4sNwFhG9rU3vKj6mE0eXcQl8n2", "Employee", "tran.thi.mai" },
                    { "ACC003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hung.nv@baixetranphu.vn", true, "$2a$12$XpV8cBnqRT5HaK2dLM7OiuZqY1P4sNwFhG9rU3vKj6mE0eXcQl8n2", "Employee", "le.van.hung" },
                    { "ACC004", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "baopq@gmail.com", true, "$2a$12$9kM4pJ3wC8zN2vR7xF5qEuY6tL0sG1hB4dI8nK3oS2aA7cW9mP1y0", "Customer", "pham.quoc.bao" },
                    { "ACC005", new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "lanhothuy@gmail.com", true, "$2a$12$9kM4pJ3wC8zN2vR7xF5qEuY6tL0sG1hB4dI8nK3oS2aA7cW9mP1y0", "Customer", "ho.thi.lan" },
                    { "ACC006", new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "khoaRacing2001@gmail.com", true, "$2a$12$9kM4pJ3wC8zN2vR7xF5qEuY6tL0sG1hB4dI8nK3oS2aA7cW9mP1y0", "Customer", "dang.minh.khoa" },
                    { "ACC007", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "thuhuong.ntn@yahoo.com", true, "$2a$12$9kM4pJ3wC8zN2vR7xF5qEuY6tL0sG1hB4dI8nK3oS2aA7cW9mP1y0", "Customer", "nguyen.thu.huong" }
                });

            migrationBuilder.InsertData(
                table: "ParkingSlots",
                columns: new[] { "SlotId", "LastUpdated", "Location", "Status", "VehicleType" },
                values: new object[,]
                {
                    { "A01", new DateTime(2026, 5, 10, 8, 23, 0, 0, DateTimeKind.Unspecified), "Khu A - Ô 01", "Đang sử dụng", "Xe máy" },
                    { "A02", new DateTime(2026, 5, 9, 17, 30, 0, 0, DateTimeKind.Unspecified), "Khu A - Ô 02", "Trống", "Xe máy" },
                    { "A03", new DateTime(2026, 5, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "Khu A - Ô 03", "Trống", "Xe máy" },
                    { "A04", new DateTime(2026, 5, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "Khu A - Ô 04", "Đã đặt", "Xe máy" },
                    { "A05", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khu A - Ô 05", "Trống", "Xe máy" },
                    { "B01", new DateTime(2026, 5, 10, 9, 5, 0, 0, DateTimeKind.Unspecified), "Khu B - Ô 01", "Đang sử dụng", "Ô tô nhỏ" },
                    { "B02", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khu B - Ô 02", "Trống", "Ô tô nhỏ" },
                    { "B03", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khu B - Ô 03", "Trống", "Ô tô nhỏ" },
                    { "C01", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khu C - Ô 01", "Đang sử dụng", "Ô tô lớn" },
                    { "C02", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khu C - Ô 02", "Trống", "Ô tô lớn" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "AccountId", "FullName", "IsDeleted", "PhoneNumber" },
                values: new object[,]
                {
                    { "CUS001", "ACC004", "Phạm Quốc Bảo", false, "0931122334" },
                    { "CUS002", "ACC005", "Hồ Thị Lan", false, "0942233445" },
                    { "CUS003", "ACC006", "Đặng Minh Khoa", false, "0953344556" },
                    { "CUS004", "ACC007", "Nguyễn Thị Thu Hương", false, "0964455667" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "AccountId", "FullName", "IsDeleted", "PhoneNumber", "Shift" },
                values: new object[,]
                {
                    { "EMP001", "ACC001", "Nguyễn Thành Tùng", false, "0901234567", "Sáng" },
                    { "EMP002", "ACC002", "Trần Thị Mai", false, "0912345678", "Chiều" },
                    { "EMP003", "ACC003", "Lê Văn Hùng", false, "0923456789", "Tối" }
                });

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "ManagerId", "Department" },
                values: new object[] { "EMP001", "Vận hành" });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "VehiclePlate", "CustomerId", "VehicleType" },
                values: new object[,]
                {
                    { "43A-123.45", "CUS001", "Xe máy" },
                    { "43B-456.78", "CUS002", "Ô tô nhỏ" },
                    { "43C-789.01", "CUS003", "Xe máy" },
                    { "43D-234.56", "CUS004", "Ô tô lớn" },
                    { "51G-888.88", "CUS001", "Ô tô nhỏ" }
                });

            migrationBuilder.InsertData(
                table: "MonthlyTickets",
                columns: new[] { "MonthlyTicketId", "CreatedAt", "CustomerId", "EndDate", "PackageType", "StartDate", "Status", "TotalFee", "VehiclePlate", "VehicleType" },
                values: new object[,]
                {
                    { "MTK001", new DateTime(2026, 5, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "CUS004", new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "1 tháng", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoạt động", 350000m, "43D-234.56", "Ô tô lớn" },
                    { "MTK002", new DateTime(2026, 4, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "CUS001", new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "3 tháng", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoạt động", 650000m, "51G-888.88", "Ô tô nhỏ" },
                    { "MTK003", new DateTime(2026, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "CUS003", new DateTime(2026, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "1 tháng", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hết hạn", 150000m, "43C-789.01", "Xe máy" }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "ReservationId", "CreatedAt", "CustomerId", "ExpectedTime", "SlotId", "Status", "VehiclePlate" },
                values: new object[,]
                {
                    { "RES001", new DateTime(2026, 5, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "CUS003", new DateTime(2026, 5, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), "A04", "Chờ", "43C-789.01" },
                    { "RES002", new DateTime(2026, 5, 7, 20, 0, 0, 0, DateTimeKind.Unspecified), "CUS002", new DateTime(2026, 5, 8, 8, 0, 0, 0, DateTimeKind.Unspecified), "B03", "Đã nhận", "43B-456.78" }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "TicketId", "CheckInTime", "CheckOutTime", "CustomerId", "Fee", "SlotId", "Status", "VehiclePlate", "VehicleType" },
                values: new object[,]
                {
                    { "TKT001", new DateTime(2026, 5, 10, 8, 23, 0, 0, DateTimeKind.Unspecified), null, "CUS001", 0m, "A01", "Đang trong bãi", "43A-123.45", "Xe máy" },
                    { "TKT002", new DateTime(2026, 5, 10, 9, 5, 0, 0, DateTimeKind.Unspecified), null, "CUS002", 0m, "B01", "Đang trong bãi", "43B-456.78", "Ô tô nhỏ" },
                    { "TKT004", new DateTime(2026, 5, 9, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 9, 17, 30, 0, 0, DateTimeKind.Unspecified), "CUS003", 10500m, "A02", "Đã ra", "43C-789.01", "Xe máy" },
                    { "TKT005", new DateTime(2026, 5, 8, 7, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "CUS001", 13500m, "A03", "Đã ra", "43A-123.45", "Xe máy" },
                    { "TKT006", new DateTime(2026, 5, 7, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 7, 11, 0, 0, 0, DateTimeKind.Unspecified), "CUS002", 24000m, "B02", "Đã ra", "43B-456.78", "Ô tô nhỏ" }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "Method", "MonthlyTicketId", "PaymentTime", "Status", "TicketId" },
                values: new object[,]
                {
                    { "PAY001", 10500m, "Tiền mặt", null, new DateTime(2026, 5, 9, 17, 31, 0, 0, DateTimeKind.Unspecified), "Thành công", "TKT004" },
                    { "PAY002", 13500m, "Chuyển khoản", null, new DateTime(2026, 5, 8, 12, 1, 0, 0, DateTimeKind.Unspecified), "Thành công", "TKT005" },
                    { "PAY003", 24000m, "Tiền mặt", null, new DateTime(2026, 5, 7, 11, 1, 0, 0, DateTimeKind.Unspecified), "Thành công", "TKT006" },
                    { "PAY005", 350000m, "Ví điện tử", "MTK001", new DateTime(2026, 5, 1, 9, 5, 0, 0, DateTimeKind.Unspecified), "Thành công", null },
                    { "PAY006", 650000m, "Chuyển khoản", "MTK002", new DateTime(2026, 4, 1, 10, 5, 0, 0, DateTimeKind.Unspecified), "Thành công", null },
                    { "PAY007", 150000m, "Tiền mặt", "MTK003", new DateTime(2026, 3, 1, 8, 5, 0, 0, DateTimeKind.Unspecified), "Thành công", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Username",
                table: "Accounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AccountId",
                table: "Customers",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AccountId",
                table: "Employees",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyTickets_CustomerId",
                table: "MonthlyTickets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyTickets_VehiclePlate",
                table: "MonthlyTickets",
                column: "VehiclePlate");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MonthlyTicketId",
                table: "Payments",
                column: "MonthlyTicketId",
                unique: true,
                filter: "[MonthlyTicketId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TicketId",
                table: "Payments",
                column: "TicketId",
                unique: true,
                filter: "[TicketId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SlotId",
                table: "Reservations",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VehiclePlate",
                table: "Reservations",
                column: "VehiclePlate");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CustomerId",
                table: "Tickets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SlotId",
                table: "Tickets",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_VehiclePlate",
                table: "Tickets",
                column: "VehiclePlate");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CustomerId",
                table: "Vehicles",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "MonthlyTickets");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "ParkingSlots");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
