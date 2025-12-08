using HospitalSupplyChainManagementSystem.Data;
using HospitalSupplyChainManagementSystem.Models;
using HospitalSupplyChainManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HospitalSupplyChainManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IInventoryService _inventory;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            IInventoryService inventory,
            ApplicationDbContext context)
        {
            _logger = logger;
            _inventory = inventory;
            _context = context;
        }

        // -----------------------------
        // Existing Index action
        // -----------------------------
        public async Task<IActionResult> Index()
        {
            var dashboard = await _inventory.GetDashboardSummaryAsync();

            _logger.LogInformation(
                "Dashboard loaded. CorrelationId: {CorrelationId}",
                HttpContext.TraceIdentifier);

            return View(dashboard);
        }


        // -----------------------------
        // Privacy View (default MVC)
        // -----------------------------
        public IActionResult Privacy()
        {
            return View();
        }

        // -----------------------------
        // Week 15 Stored Procedure Call
        // GET: /Home/LowStock?threshold=10
        // -----------------------------
        public async Task<IActionResult> LowStock(int threshold = 10, CancellationToken ct = default)
        {
            _logger.LogInformation(
                "Loading low stock products with threshold {Threshold}. CorrelationId: {CorrelationId}",
                threshold,
                HttpContext.TraceIdentifier);

            // Call the stored procedure safely using EF Core
            var products = await _context.Products
                .FromSqlInterpolated($"EXEC dbo.GetLowStockProducts {threshold}")
                .AsNoTracking()
                .ToListAsync(ct);

            return View(products);
        }

        // -----------------------------
        // Default Error Page
        // -----------------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
        }
    }
}
