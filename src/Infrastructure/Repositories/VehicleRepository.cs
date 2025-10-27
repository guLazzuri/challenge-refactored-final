using challenge.Domain.Entities;
using challenge.Domain.Interfaces;

using challenge.Domain.Enums;
using System.Linq.Expressions;
using MongoDB.Driver;


namespace challenge.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IMongoRepository<Vehicle> _repository;

        public VehicleRepository(IMongoRepository<Vehicle> repository) // O construtor base é dummy, pois a injeção será feita via IMongoRepository
        {
            _repository = repository;
        }

        public async Task<Vehicle?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
        {
            var filter = Builders<Vehicle>.Filter.Eq(v => v.LicensePlate, licensePlate.ToUpper());
            return await _repository.FindAsync(v => v.LicensePlate == licensePlate.ToUpper()).ContinueWith(t => t.Result.FirstOrDefault());
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetByStatusAsync(VehicleStatus status)
        {
            return await _repository.FindAsync(v => v.Status == status);
        }

        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync()
        {
            return await _repository.FindAsync(v => v.Status == VehicleStatus.Available);
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _repository.AddAsync(vehicle);
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            await _repository.UpdateAsync(vehicle);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var vehicle = await _repository.GetByIdAsync(id);
            return vehicle != null;
        }

        public async Task<bool> LicensePlateExistsAsync(string licensePlate)
        {
            var vehicle = await _repository.FindAsync(v => v.LicensePlate == licensePlate.ToUpper()).ContinueWith(t => t.Result.FirstOrDefault());
            return vehicle != null;
        }
    }
}
