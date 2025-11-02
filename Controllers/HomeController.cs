using HospitalSupplyChainManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HospitalSupplyChainManagementSystem.Services;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HospitalSupplyChainManagementSystem.Models;
using HospitalSupplyChainManagementSystem.Services;  // <-- needed for IInventoryService

namespace HospitalSupplyChainManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IInventoryService _inventory;   // <-- make sure this field exists

        // NOTE the comma between parameters and exact casing of IInventoryService
        public HomeController(ILogger<HomeController> logger, IInventoryService inventory)
        {
            _logger = logger;
            _inventory = inventory;  // <-- assigns the injected service to the field
        }

        // Make the action async and return the service result to the view
        public async Task<IActionResult> Index()
        {
            var summary = await _inventory.GetDashboardSummaryAsync();
            return View(summary);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
