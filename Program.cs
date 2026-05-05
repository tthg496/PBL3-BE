using Microsoft.EntityFrameworkCore;
using ParkingManagement.BLL.DTOs;
using ParkingManagement.BLL.Services.Implementations;
using ParkingManagement.BLL.Services.Interfaces;
using ParkingManagement.DAL.Data;
using ParkingManagement.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// VertifyOTP
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

// Add all services (DbContext, Repositories, Services)
// ✅ Chỉ dùng AddApplicationServices — đã đăng ký AppDbContext bên trong rồi
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});

// Controllers + Session
builder.Services.AddControllersWithViews();
builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(30);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Parking Management API",
        Version = "v1",
        Description = "API"
    });
    c.CustomOperationIds(api => api.ActionDescriptor.RouteValues["controller"] + "_" + api.HttpMethod + "_" + api.RelativePath);
});

var app = builder.Build();

// ✅ Auto migrate + seed — dùng AppDbContext
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database migration was skipped. Check the SQL Server connection before using data endpoints. {ex.Message}");
    }
}

// ... phần còn lại giữ nguyên

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("Frontend");
app.UseSession();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "legacy",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
app.MapFallbackToFile("index.html");

app.Run();
