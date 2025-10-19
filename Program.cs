using HospitalSupplyChainManagementSystem.Data;   // ✅ add this namespace for your DbContext
using Microsoft.EntityFrameworkCore;               // ✅ add this for EF Core

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Add MVC services to the container
builder.Services.AddControllersWithViews();

// 2️⃣ Register Entity Framework DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 👉 If you are using SQLite instead of SQL Server, comment the above and uncomment this:
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 3️⃣ Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// 4️⃣ Default MVC route mapping
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
