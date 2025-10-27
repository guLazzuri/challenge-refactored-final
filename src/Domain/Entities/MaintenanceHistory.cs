using challenge.Domain.Enums;


namespace challenge.Domain.Entities
{
    public class MaintenanceHistory : MongoEntity
    {
        
        public string VehicleId { get; private set; }
        public Vehicle Vehicle { get; private set; } = null!;
        public string Description { get; private set; }
        public decimal Cost { get; private set; }
        public DateTime MaintenanceDate { get; private set; }
        public MaintenanceType Type { get; private set; }
        public string? Notes { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected MaintenanceHistory() 
        { 
            Vehicle = null!;
            Description = string.Empty;
        } // Para EF Core

        public MaintenanceHistory(string vehicleId, string description, decimal cost, MaintenanceType type, string? notes = null)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("ID do veículo deve ser maior que zero", nameof(vehicleId));
            
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição não pode ser vazia", nameof(description));
            
            if (cost < 0)
                throw new ArgumentException("Custo não pode ser negativo", nameof(cost));

            VehicleId = vehicleId;
            Description = description.Trim();
            Cost = cost;
            Type = type;
            Notes = notes?.Trim();
            MaintenanceDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateNotes(string notes)
        {
            Notes = notes?.Trim();
        }

        public bool IsPreventive() => Type == MaintenanceType.Preventive;
        
        public bool IsCorrective() => Type == MaintenanceType.Corrective;
        
        public bool IsExpensive() => Cost > 1000; // Valor arbitrário para manutenção cara
    }
}

