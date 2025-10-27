using challenge.Application.DTOs;
using challenge.Domain.Entities;
using challenge.Domain.Interfaces;
using challenge.Domain.ValueObjects;
using challenge.Domain.Enums;

namespace challenge.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createDto)
        {
            if (await _userRepository.EmailExistsAsync(createDto.Email))
                throw new InvalidOperationException("Já existe um usuário com este email");

            if (await _userRepository.DocumentExistsAsync(createDto.Document))
                throw new InvalidOperationException("Já existe um usuário com este documento");

            var email = new Email(createDto.Email);
            var user = new User(createDto.Name, email, createDto.Document, createDto.Type);

            await _userRepository.AddAsync(user);
var createdUser = user;
            return MapToDto(createdUser);
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> GetUserByDocumentAsync(string document)
        {
            var user = await _userRepository.GetByDocumentAsync(document);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<IEnumerable<UserDto>> GetActiveUsersAsync()
        {
            var users = await _userRepository.GetActiveUsersAsync();
            return users.Select(MapToDto);
        }

        public async Task<IEnumerable<UserDto>> GetUsersByTypeAsync(UserType type)
        {
            var users = await _userRepository.GetByTypeAsync(type);
            return users.Select(MapToDto);
        }

       public async Task<UserDto> UpdateUserAsync(string id, UpdateUserDto updateDto)       {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new ArgumentException("Usuário não encontrado", nameof(id));

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
                user.UpdateName(updateDto.Name);

            if (!string.IsNullOrWhiteSpace(updateDto.Email))
            {
                if (await _userRepository.EmailExistsAsync(updateDto.Email))
                    throw new InvalidOperationException("Já existe um usuário com este email");
                
                var newEmail = new Email(updateDto.Email);
                user.UpdateEmail(newEmail);
            }

            await _userRepository.UpdateAsync(user);
            return MapToDto(user);
        }

        public async Task<UserDto> DeactivateUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new ArgumentException("Usuário não encontrado", nameof(id));

            user.Deactivate();
            await _userRepository.UpdateAsync(user);
            return MapToDto(user);
        }

        public async Task<UserDto> ActivateUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new ArgumentException("Usuário não encontrado", nameof(id));

            user.Activate();
            await _userRepository.UpdateAsync(user);
            return MapToDto(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new ArgumentException("Usuário não encontrado", nameof(id));

            if (user.GetRentedVehiclesCount() > 0)
                throw new InvalidOperationException("Não é possível excluir usuário com veículos alugados");

            await _userRepository.DeleteAsync(id);
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email.Value,
                Document = user.Document,
                Type = user.Type,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}

