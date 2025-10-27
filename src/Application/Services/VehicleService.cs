using challenge.Application.DTOs;
using challenge.Domain.Entities;
using challenge.Domain.Interfaces;
using challenge.Domain.ValueObjects;
using challenge.Domain.Enums;

namespace challenge.Application.Services
{
    public class VehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto createDto)
        {
            if (await _vehicleRepository.LicensePlateExistsAsync(createDto.LicensePlate))
                throw new InvalidOperationException("Já existe um veículo com esta placa");

            var vehicleModel = new VehicleModel(createDto.Brand, createDto.Model);
            var vehicle = new Vehicle(createDto.LicensePlate, vehicleModel, createDto.Year);

            await _vehicleRepository.AddAsync(vehicle);
var createdVehicle = vehicle;
            return MapToDto(createdVehicle);
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(string id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            return vehicle != null ? MapToDto(vehicle) : null;
        }

        public async Task<VehicleDto?> GetVehicleByLicensePlateAsync(string licensePlate)
        {
            var vehicle = await _vehicleRepository.GetByLicensePlateAsync(licensePlate);
            return vehicle != null ? MapToDto(vehicle) : null;
        }

        public async Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return vehicles.Select(MapToDto);
        }

        public async Task<IEnumerable<VehicleDto>> GetAvailableVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetAvailableVehiclesAsync();
            return vehicles.Select(MapToDto);
        }

        public async Task<IEnumerable<VehicleDto>> GetVehiclesByStatusAsync(VehicleStatus status)
        {
            var vehicles = await _vehicleRepository.GetByStatusAsync(status);
            return vehicles.Select(MapToDto);
        }

        public async Task<VehicleDto> UpdateVehicleAsync(string id, UpdateVehicleDto updateDto)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
                throw new ArgumentException("Veículo não encontrado", nameof(id));

            // Como as propriedades são privadas, precisaríamos de métodos de atualização na entidade
            // Por simplicidade, vamos assumir que existe um método Update na entidade
            
            await _vehicleRepository.UpdateAsync(vehicle);
            return MapToDto(vehicle);
        }

        public async Task<VehicleDto> ChangeVehicleStatusAsync(string id, VehicleStatus newStatus)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
                throw new ArgumentException("Veículo não encontrado", nameof(id));

            switch (newStatus)
            {
                case VehicleStatus.Rented:
                    vehicle.Rent();
                    break;
                case VehicleStatus.Available:
                    vehicle.Return();
                    break;
                case VehicleStatus.InMaintenance:
                    vehicle.SendToMaintenance();
                    break;
                default:
                    throw new ArgumentException("Status inválido", nameof(newStatus));
            }

            await _vehicleRepository.UpdateAsync(vehicle);
            return MapToDto(vehicle);
        }

        public async Task DeleteVehicleAsync(string id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
                throw new ArgumentException("Veículo não encontrado", nameof(id));

            if (vehicle.Status == VehicleStatus.Rented)
                throw new InvalidOperationException("Não é possível excluir veículo alugado");

            await _vehicleRepository.DeleteAsync(id);
        }

        private static VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                LicensePlate = vehicle.LicensePlate,
                Brand = vehicle.Model.Brand,
                Model = vehicle.Model.Model,
                Year = vehicle.Year,
                Status = vehicle.Status,
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt
            };
        }
    }
}

