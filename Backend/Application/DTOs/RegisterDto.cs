using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Nombre de la empresa para crear el Tenant
        public string BusinessName { get; set; } = string.Empty;
    }
}