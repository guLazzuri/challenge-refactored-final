using challenge.Domain.ValueObjects;
using challenge.Domain.Enums;


namespace challenge.Domain.Entities
{
    public class Vehicle : MongoEntity
    {
        
        public string LicensePlate { get; private set; }
        public VehicleModel Model { get; private set; }
        public int Year { get; private set; }
        public VehicleStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        
        // Lista de históricos de manutenção será gerenciada pelo agregado
        // private readonly List<MaintenanceHistory> _maintenanceHistories = new();
        // public IReadOnlyCollection<MaintenanceHistory> MaintenanceHistories => _maintenanceHistories.AsReadOnly();

        protected Vehicle() 
        { 
            LicensePlate = string.Empty;
            Model = null!;
        } // Para EF Core

        public Vehicle(string licensePlate, VehicleModel model, int year)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                throw new ArgumentException("Placa não pode ser vazia", nameof(licensePlate));
            
            if (year < 1900 || year > DateTime.Now.Year + 1)
                throw new ArgumentException("Ano inválido", nameof(year));

            LicensePlate = licensePlate.ToUpper();
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Year = year;
            Status = VehicleStatus.Available;
            CreatedAt = DateTime.UtcNow;
        }

        public void Rent()
        {
            if (Status != VehicleStatus.Available)
                throw new InvalidOperationException("Veículo não está disponível para locação");
            
            Status = VehicleStatus.Rented;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Return()
        {
            if (Status != VehicleStatus.Rented)
                throw new InvalidOperationException("Veículo não está alugado");
            
            Status = VehicleStatus.Available;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SendToMaintenance()
        {
            if (Status == VehicleStatus.InMaintenance)
                throw new InvalidOperationException("Veículo já está em manutenção");
            
            Status = VehicleStatus.InMaintenance;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CompleteMaintenance()
        {
            if (Status != VehicleStatus.InMaintenance)
                throw new InvalidOperationException("Veículo não está em manutenção");
            
            Status = VehicleStatus.Available;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddMaintenanceHistory(MaintenanceHistory maintenance)
        {
            // Este método será implementado no agregado
            throw new NotImplementedException("Use VehicleAggregate para gerenciar histórico de manutenção");
        }

        public bool IsAvailable() => Status == VehicleStatus.Available;
        
        public bool RequiresMaintenance()
        {
            // Lógica simplificada - pode ser expandida com dados do repositório
            return false; // Implementação básica
        }
    }
}
