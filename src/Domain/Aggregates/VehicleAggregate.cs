using challenge.Domain.Entities;
using challenge.Domain.ValueObjects;
using challenge.Domain.Enums;

namespace challenge.Domain.Aggregates
{
    /// <summary>
    /// Agregado Raiz para Vehicle - controla a consistência do veículo e seus históricos de manutenção
    /// </summary>
    public class VehicleAggregate
    {
        private readonly Vehicle _vehicle;
        private readonly List<MaintenanceHistory> _maintenanceHistories;

        public Vehicle Vehicle => _vehicle;
        public IReadOnlyCollection<MaintenanceHistory> MaintenanceHistories => _maintenanceHistories.AsReadOnly();

        public VehicleAggregate(Vehicle vehicle, IEnumerable<MaintenanceHistory>? maintenanceHistories = null)
        {
            _vehicle = vehicle ?? throw new ArgumentNullException(nameof(vehicle));
            _maintenanceHistories = maintenanceHistories?.ToList() ?? new List<MaintenanceHistory>();
        }

        public void ScheduleMaintenance(string description, decimal estimatedCost, MaintenanceType type, string? notes = null)
        {
            // Regra de negócio: não pode agendar manutenção se o veículo estiver alugado
            if (_vehicle.Status == VehicleStatus.Rented)
                throw new InvalidOperationException("Não é possível agendar manutenção para veículo alugado");

            var maintenance = new MaintenanceHistory(_vehicle.Id, description, estimatedCost, type, notes);
            _maintenanceHistories.Add(maintenance);
            
            _vehicle.SendToMaintenance();
        }

        public void CompleteMaintenance(string maintenanceId, decimal actualCost, string? completionNotes = null)
        {
            var maintenance = _maintenanceHistories.FirstOrDefault(m => m.Id == maintenanceId);
            if (maintenance == null)
                throw new ArgumentException("Manutenção não encontrada", nameof(maintenanceId));

            if (_vehicle.Status != VehicleStatus.InMaintenance)
                throw new InvalidOperationException("Veículo não está em manutenção");

            // Atualizar notas da manutenção
            if (!string.IsNullOrWhiteSpace(completionNotes))
                maintenance.UpdateNotes(completionNotes);

            _vehicle.CompleteMaintenance();
        }

        public decimal GetTotalMaintenanceCost()
        {
            return _maintenanceHistories.Sum(m => m.Cost);
        }

        public decimal GetMaintenanceCostByPeriod(DateTime startDate, DateTime endDate)
        {
            return _maintenanceHistories
                .Where(m => m.MaintenanceDate >= startDate && m.MaintenanceDate <= endDate)
                .Sum(m => m.Cost);
        }

        public bool HasRecentMaintenance(int months = 6)
        {
            var cutoffDate = DateTime.UtcNow.AddMonths(-months);
            return _maintenanceHistories.Any(m => m.MaintenanceDate >= cutoffDate);
        }

        public void ValidateForRental()
        {
            if (!_vehicle.IsAvailable())
                throw new InvalidOperationException("Veículo não está disponível para locação");

            if (_vehicle.RequiresMaintenance())
                throw new InvalidOperationException("Veículo requer manutenção antes de ser alugado");
        }
    }
}

