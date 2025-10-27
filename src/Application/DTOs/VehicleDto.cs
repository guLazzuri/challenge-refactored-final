using challenge.Domain.Enums;

namespace challenge.Application.DTOs
{
    public class VehicleDto
    {
        public string Id { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public VehicleStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateVehicleDto
    {
        public string LicensePlate { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
    }

    public class UpdateVehicleDto
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
    }

    public class VehicleStatusDto
    {
        public VehicleStatus Status { get; set; }
    }
}

