using challenge.Domain.Entities;
using challenge.Domain.Interfaces;

using challenge.Domain.Enums;
using System.Linq.Expressions;
using MongoDB.Driver;


namespace challenge.Infrastructure.Repositories
{
    public class MaintenanceHistoryRepository : IMaintenanceHistoryRepository
    {
        private readonly IMongoRepository<MaintenanceHistory> _repository;

        // O construtor base é dummy
        public MaintenanceHistoryRepository(IMongoRepository<MaintenanceHistory> repository) 
        {
            _repository = repository;
        }

        public async Task<MaintenanceHistory?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetByVehicleIdAsync(string vehicleId)
        {
            return await _repository.FindAsync(m => m.VehicleId == vehicleId);
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetByTypeAsync(MaintenanceType type)
        {
            return await _repository.FindAsync(m => m.Type == type);
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _repository.FindAsync(m => m.MaintenanceDate >= startDate && m.MaintenanceDate <= endDate);
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(MaintenanceHistory maintenanceHistory)
        {
            await _repository.AddAsync(maintenanceHistory);
        }

        public async Task UpdateAsync(MaintenanceHistory maintenanceHistory)
        {
            await _repository.UpdateAsync(maintenanceHistory);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var history = await _repository.GetByIdAsync(id);
            return history != null;
        }

        public async Task<decimal> GetTotalCostByVehicleAsync(string vehicleId)
        {
            var history = await GetByVehicleIdAsync(vehicleId);
            return history.Sum(m => m.Cost);
        }

        public async Task<decimal> GetTotalCostByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var history = await GetByPeriodAsync(startDate, endDate);
            return history.Sum(m => m.Cost);
        }
    }
}
