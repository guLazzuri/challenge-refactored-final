namespace challenge.Domain.ValueObjects
{
    public class VehicleModel
    {
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public string FullName => $"{Brand} {Model}";

        protected VehicleModel() 
        { 
            Brand = string.Empty;
            Model = string.Empty;
        } // Para EF Core

        public VehicleModel(string brand, string model)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Marca não pode ser vazia", nameof(brand));
            
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Modelo não pode ser vazio", nameof(model));

            Brand = brand.Trim();
            Model = model.Trim();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not VehicleModel other)
                return false;

            return Brand.Equals(other.Brand, StringComparison.OrdinalIgnoreCase) &&
                   Model.Equals(other.Model, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                Brand.ToLowerInvariant(),
                Model.ToLowerInvariant()
            );
        }

        public override string ToString() => FullName;

        public static implicit operator string(VehicleModel vehicleModel) => vehicleModel.FullName;
    }
}

