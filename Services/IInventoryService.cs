using System.Threading.Tasks;
using HospitalSupplyChainManagementSystem.Models;

namespace HospitalSupplyChainManagementSystem.Services
{
    /// Service contract for non-UI inventory/business logic.
    public interface IInventoryService
    {
        Task<DashboardSummary> GetDashboardSummaryAsync();
    }
}
