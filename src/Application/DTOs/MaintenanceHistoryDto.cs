using challenge.Domain.Enums;

namespace challenge.Application.DTOs
{
    public class MaintenanceHistoryDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public MaintenanceType Type { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateMaintenanceHistoryDto
    {
        public int VehicleId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public MaintenanceType Type { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateMaintenanceHistoryDto
    {
        public string? Notes { get; set; }
    }
}

