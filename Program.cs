using HospitalSupplyChainManagementSystem.Data;
using HospitalSupplyChainManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// Logging (Structured + Console)
// ---------------------------------------------------------
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// ---------------------------------------------------------
// MVC
// ---------------------------------------------------------
builder.Services.AddControllersWithViews();

// ---------------------------------------------------------
// EF Core DbContext
// ---------------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------------------------------------------
// Business Services (DI)
// ---------------------------------------------------------
builder.Services.AddScoped<IInventoryService, InventoryService>();

// ---------------------------------------------------------
// Health Checks
// ---------------------------------------------------------
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("HSCMS-Database");

// ---------------------------------------------------------
// Build App
// ---------------------------------------------------------
var app = builder.Build();

// ---------------------------------------------------------
// Ensure database exists (bypasses broken EF tools)
// ---------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// ---------------------------------------------------------
// Structured Logging Scope (Correlation ID)
// ---------------------------------------------------------
var logger = app.Logger;

app.Use(async (context, next) =>
{
    using (logger.BeginScope(new Dictionary<string, object?>
    {
        ["CorrelationId"] = context.TraceIdentifier,
        ["RequestPath"] = context.Request.Path,
        ["Method"] = context.Request.Method
    }))
    {
        await next();
    }
});

// ---------------------------------------------------------
// HTTP Pipeline
// ---------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ---------------------------------------------------------
// /healthz Endpoint
// ---------------------------------------------------------
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

// ---------------------------------------------------------
// MVC Routing
// ---------------------------------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
