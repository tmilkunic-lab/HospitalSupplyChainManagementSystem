using HospitalSupplyChainManagementSystem.Data;          // DbContext
using HospitalSupplyChainManagementSystem.Services;      // IInventoryService, InventoryService
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// EF Core DbContext  ---- PICK ONE ----
// SQL Server:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// If using SQLite instead, comment the SQL Server block above and uncomment this:
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Business services (Scoped = per-request)
builder.Services.AddScoped<IInventoryService, InventoryService>();
// builder.Services.AddScoped<IOtherService, OtherService>();

// Build AFTER all registrations
var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
