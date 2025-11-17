using HospitalSupplyChainManagementSystem.Data;
using HospitalSupplyChainManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// EF Core DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Business services
builder.Services.AddScoped<IInventoryService, InventoryService>();

// Health checks – real DB dependency
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("HSCMS-Database");


// Build
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

// /healthz endpoint
app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                component = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                duration_ms = entry.Value.Duration.TotalMilliseconds
            }),
            totalDuration_ms = report.TotalDuration.TotalMilliseconds
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
