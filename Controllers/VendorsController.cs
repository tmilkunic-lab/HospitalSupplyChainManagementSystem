// File: Controllers/VendorsController.cs
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HospitalSupplyChainManagementSystem.Data;     // your DbContext namespace
using HospitalSupplyChainManagementSystem.Models;   // your Vendor model namespace

namespace HospitalSupplyChainManagementSystem.Controllers
{
    public class VendorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VendorsController> _logger;

        public VendorsController(ApplicationDbContext context,
                                 ILogger<VendorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Vendors
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var vendors = await _context.Vendors
                .AsNoTracking()
                .ToListAsync(ct);   // ToListAsync

            _logger.LogInformation(
                "Vendor list loaded successfully. Count: {Count}, CorrelationId: {CorrelationId}",
                vendors.Count,
                HttpContext.TraceIdentifier);

            return View(vendors);
        }

        // GET: Vendors/Details/5
        public async Task<IActionResult> Details(int? id, CancellationToken ct)
        {
            if (id == null)
            {
                _logger.LogWarning(
                    "Vendor details requested with null id. CorrelationId: {CorrelationId}",
                    HttpContext.TraceIdentifier);

                return NotFound();
            }

            var vendor = await _context.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, ct);   // FirstOrDefaultAsync

            if (vendor == null)
            {
                _logger.LogWarning(
                    "Vendor not found. VendorId: {VendorId}, CorrelationId: {CorrelationId}",
                    id,
                    HttpContext.TraceIdentifier);

                return NotFound();
            }

            _logger.LogInformation(
                "Vendor details loaded. VendorId: {VendorId}, CorrelationId: {CorrelationId}",
                id,
                HttpContext.TraceIdentifier);

            return View(vendor);
        }

        // GET: Vendors/Create
        public IActionResult Create()
        {
            _logger.LogInformation(
                "Vendor create form displayed. CorrelationId: {CorrelationId}",
                HttpContext.TraceIdentifier);

            return View();
        }

        // POST: Vendors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Description,Phone,Email,Website,Status")] Vendor vendor,
            CancellationToken ct)  // prevent overposting
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Vendor create model state invalid. CorrelationId: {CorrelationId}",
                    HttpContext.TraceIdentifier);

                return View(vendor);
            }

            try
            {
                _context.Add(vendor);
                await _context.SaveChangesAsync(ct);   // SaveChangesAsync

                _logger.LogInformation(
                    "Vendor created successfully. VendorId: {VendorId}, CorrelationId: {CorrelationId}",
                    vendor.Id,
                    HttpContext.TraceIdentifier);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating vendor. CorrelationId: {CorrelationId}",
                    HttpContext.TraceIdentifier);

                ModelState.AddModelError(
                    string.Empty,
                    "Unable to save vendor. Try again, and if the problem continues contact support.");

                return View(vendor);
            }
        }

        // GET: Vendors/Edit/5
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var vendor = await _context.Vendors.FindAsync(new object[] { id }, ct);   // FindAsync
            if (vendor == null)
            {
                _logger.LogWarning(
                    "Vendor edit requested for missing vendor. VendorId: {VendorId}, CorrelationId: {CorrelationId}",
                    id,
                    HttpContext.TraceIdentifier);

                return NotFound();
            }

            return View(vendor);
        }

        // POST: Vendors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Description,Phone,Email,Website,Status")] Vendor vendor,
            CancellationToken ct)
        {
            if (id != vendor.Id)
            {
                _logger.LogWarning(
                    "Vendor edit id mismatch. RouteId: {RouteId}, ModelId: {ModelId}, CorrelationId: {CorrelationId}",
                    id,
                    vendor.Id,
                    HttpContext.TraceIdentifier);

                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Vendor edit model state invalid. VendorId: {VendorId}, CorrelationId: {CorrelationId}",
                    vendor.Id,
                    HttpContext.TraceIdentifier);

                return View(vendor);
            }

            try
            {
                _context.Update(vendor);
                await _context.SaveChangesAsync(ct);

                _logger.LogInformation(
                    "Vendor updated successfully. VendorId: {VendorId}, CorrelationId: {CorrelationId}",
                    vendor.Id,
                    HttpContext.TraceIdentifier);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await VendorExists(vendor.Id, ct))
                {
                    _logger.LogWarning(
                        "Vendor edit failed because vendor no longer exists. VendorId: {VendorId}, CorrelationId: {CorrelationId}",
                        vendor.Id,
                        HttpContext.TraceIdentifier);

                    return NotFound();
                }

                _logger.LogError(
                    ex,
                    "Concurrency error updating vendor. VendorId: {VendorId}, CorrelationId: {CorrelationId}",
                    vendor.Id,
                    HttpContext.TraceIdentifier);

                throw;
            }
        }

        // GET: Vendors/Delete/5
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var vendor = await _context.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, ct);

            if (vendor == null)
            {
                _logger.LogWarning(
                    "Vendor delete requested for missing vendor. VendorId: {VendorId}, CorrelationId: {CorrelationId}",
                    id,
                    HttpContext.TraceIdentifier);

                return NotFound();
            }

            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var vendor = await _context.Vendors.FindAsync(new object[] { id }, ct);

            if (vendor == null)
            {
                _logger.LogWarning(
                    "Vendor delete confirmed but vendor already missing. VendorId: {VendorId}, CorrelationId: {CorrelationId}",
                    id,
                    HttpContext.TraceIdentifier);

                return RedirectToAction(nameof(Index));
            }

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation(
                "Vendor deleted successfully. VendorId: {VendorId}, CorrelationId: {CorrelationId}",
                id,
                HttpContext.TraceIdentifier);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> VendorExists(int id, CancellationToken ct)
        {
            return await _context.Vendors.AnyAsync(e => e.Id == id, ct);
        }
    }
}
