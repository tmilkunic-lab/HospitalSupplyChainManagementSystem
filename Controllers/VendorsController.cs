// File: Controllers/VendorsController.cs
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSupplyChainManagementSystem.Data;     // <-- your DbContext namespace
using HospitalSupplyChainManagementSystem.Models;   // <-- your Vendor model namespace

namespace HospitalSupplyChainManagementSystem.Controllers
{
    public class VendorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendors
        // async read with AsNoTracking + ToListAsync
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var vendors = await _context.Vendors
                .AsNoTracking()
                .ToListAsync(ct);                 // <-- ToListAsync
            return View(vendors);
        }

        // GET: Vendors/Details/5
        public async Task<IActionResult> Details(int? id, CancellationToken ct)
        {
            if (id == null) return NotFound();

            var vendor = await _context.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, ct); // <-- FirstOrDefaultAsync
            if (vendor == null) return NotFound();

            return View(vendor);
        }

        // GET: Vendors/Create
        public IActionResult Create() => View();

        // POST: Vendors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,ContactName,Phone,Email,Website,Status")] Vendor vendor, // prevent overposting
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vendor);

            _context.Add(vendor);
            await _context.SaveChangesAsync(ct);       // <-- SaveChangesAsync
            return RedirectToAction(nameof(Index));
        }

        // GET: Vendors/Edit/5
        public async Task<IActionResult> Edit(int? id, CancellationToken ct)
        {
            if (id == null) return NotFound();

            var vendor = await _context.Vendors.FindAsync(new object?[] { id }, ct); // <-- FindAsync
            if (vendor == null) return NotFound();

            return View(vendor);
        }

        // POST: Vendors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,ContactName,Phone,Email,Website,Status")] Vendor vendor,
            CancellationToken ct)
        {
            if (id != vendor.Id) return NotFound();
            if (!ModelState.IsValid) return View(vendor);

            try
            {
                _context.Update(vendor);
                await _context.SaveChangesAsync(ct);    // <-- SaveChangesAsync
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await VendorExistsAsync(vendor.Id, ct))
                    return NotFound();
                throw; // rethrow so you see the issue in dev
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Vendors/Delete/5
        public async Task<IActionResult> Delete(int? id, CancellationToken ct)
        {
            if (id == null) return NotFound();

            var vendor = await _context.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, ct);  // <-- FirstOrDefaultAsync
            if (vendor == null) return NotFound();

            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var vendor = await _context.Vendors.FindAsync(new object?[] { id }, ct); // <-- FindAsync
            if (vendor != null)
            {
                _context.Vendors.Remove(vendor);
                await _context.SaveChangesAsync(ct);       // <-- SaveChangesAsync
            }
            return RedirectToAction(nameof(Index));
        }

        // Helper
        private Task<bool> VendorExistsAsync(int id, CancellationToken ct) =>
            _context.Vendors.AnyAsync(e => e.Id == id, ct);   // <-- AnyAsync
    }
}
