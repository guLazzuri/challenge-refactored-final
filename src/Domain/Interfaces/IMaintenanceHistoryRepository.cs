using challenge.Domain.Entities;
using challenge.Domain.Enums;

namespace challenge.Domain.Interfaces
{
    public interface IMaintenanceHistoryRepository
    {
        Task<MaintenanceHistory?> GetByIdAsync(string id);
        Task<IEnumerable<MaintenanceHistory>> GetByVehicleIdAsync(string vehicleId);
        Task<IEnumerable<MaintenanceHistory>> GetByTypeAsync(MaintenanceType type);
        Task<IEnumerable<MaintenanceHistory>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MaintenanceHistory>> GetAllAsync();
        Task AddAsync(MaintenanceHistory maintenanceHistory);
        Task UpdateAsync(MaintenanceHistory maintenanceHistory);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<decimal> GetTotalCostByVehicleAsync(string vehicleId);
        Task<decimal> GetTotalCostByPeriodAsync(DateTime startDate, DateTime endDate);
    }
}

