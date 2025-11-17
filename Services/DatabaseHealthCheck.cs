using System.Threading;
using System.Threading.Tasks;
using HospitalSupplyChainManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HospitalSupplyChainManagementSystem.Services
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseHealthCheck(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Real dependency check: can the app connect to the database?
                var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);

                if (canConnect)
                {
                    return HealthCheckResult.Healthy("Database connection is healthy.");
                }

                return HealthCheckResult.Unhealthy("Database cannot be reached.");
            }
            catch (System.Exception ex)
            {
                // No secrets in the message, just a high-level error
                return HealthCheckResult.Unhealthy("Database health check failed.", ex);
            }
        }
    }
}
