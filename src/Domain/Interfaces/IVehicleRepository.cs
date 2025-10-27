using challenge.Domain.Entities;
using challenge.Domain.Enums;

namespace challenge.Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(string id);
        Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<IEnumerable<Vehicle>> GetByStatusAsync(VehicleStatus status);
        Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync();
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> LicensePlateExistsAsync(string licensePlate);
    }
}

