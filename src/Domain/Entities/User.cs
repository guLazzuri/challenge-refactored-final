using challenge.Domain.ValueObjects;
using challenge.Domain.Enums;


namespace challenge.Domain.Entities
{
    public class User : MongoEntity
    {
        
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public string Document { get; private set; }
        public UserType Type { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        // Lista de veículos alugados
        private readonly List<Vehicle> _rentedVehicles = new();
        public IReadOnlyCollection<Vehicle> RentedVehicles => _rentedVehicles.AsReadOnly();

        protected User() 
        { 
            Name = string.Empty;
            Email = null!;
            Document = string.Empty;
        } // Para EF Core

        public User(string name, Email email, string document, UserType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome não pode ser vazio", nameof(name));
            
            if (string.IsNullOrWhiteSpace(document))
                throw new ArgumentException("Documento não pode ser vazio", nameof(document));

            Name = name.Trim();
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Document = document.Trim();
            Type = type;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Nome não pode ser vazio", nameof(newName));

            Name = newName.Trim();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateEmail(Email newEmail)
        {
            Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("Usuário já está inativo");

            if (_rentedVehicles.Any())
                throw new InvalidOperationException("Não é possível desativar usuário com veículos alugados");

            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("Usuário já está ativo");

            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RentVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));

            if (!IsActive)
                throw new InvalidOperationException("Usuário inativo não pode alugar veículos");

            if (!vehicle.IsAvailable())
                throw new InvalidOperationException("Veículo não está disponível");

            vehicle.Rent();
            _rentedVehicles.Add(vehicle);
            UpdatedAt = DateTime.UtcNow;
        }

        public void ReturnVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));

            if (!_rentedVehicles.Contains(vehicle))
                throw new InvalidOperationException("Veículo não está alugado por este usuário");

            vehicle.Return();
            _rentedVehicles.Remove(vehicle);
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanRentVehicles() => IsActive && Type == UserType.Customer;
        
        public int GetRentedVehiclesCount() => _rentedVehicles.Count;
    }
}

