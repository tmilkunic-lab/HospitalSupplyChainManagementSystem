using System.Threading.Tasks;
using HospitalSupplyChainManagementSystem.Data;
using HospitalSupplyChainManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalSupplyChainManagementSystem.Services
{
    /// Concrete implementation of inventory-related business logic.
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _db;
        public InventoryService(ApplicationDbContext db) => _db = db;

        public async Task<DashboardSummary> GetDashboardSummaryAsync()
        {
            var productCount = await _db.Products.CountAsync();
            var supplierCount = await _db.Suppliers.CountAsync();
            var openOrders = await _db.Orders.CountAsync(o => o.Status == "Open");
            var pendingOrders = await _db.Orders.CountAsync(o => o.Status == "Pending");
            var fulfilledOrders = await _db.Orders.CountAsync(o => o.Status == "Fulfilled");

            return new DashboardSummary
            {
                TotalProducts = productCount,
                TotalSuppliers = supplierCount,
                OpenOrders = openOrders,
                PendingOrders = pendingOrders,
                FulfilledOrders = fulfilledOrders
            };
        }
    }
}
