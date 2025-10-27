using System.Text.RegularExpressions;

namespace challenge.Domain.ValueObjects
{
    public class Email
    {
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        public string Value { get; private set; }

        protected Email() 
        { 
            Value = string.Empty;
        } // Para EF Core

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email não pode ser vazio", nameof(value));

            var normalizedEmail = value.Trim().ToLowerInvariant();
            
            if (!EmailRegex.IsMatch(normalizedEmail))
                throw new ArgumentException("Email inválido", nameof(value));

            Value = normalizedEmail;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Email other)
                return false;

            return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() => Value;

        public static implicit operator string(Email email) => email.Value;
        
        public static explicit operator Email(string email) => new(email);
    }
}

