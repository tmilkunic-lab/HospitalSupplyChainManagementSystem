namespace HospitalSupplyChainManagementSystem.Models
{
    /// Simple DTO for the Home/Index dashboard.
    public class DashboardSummary
    {
        public int TotalProducts { get; set; }
        public int TotalSuppliers { get; set; }
        public int OpenOrders { get; set; }
        public int PendingOrders { get; set; }
        public int FulfilledOrders { get; set; }
    }
}
