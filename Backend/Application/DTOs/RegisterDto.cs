using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class RegisterDto
    {
        // Datos para armar el Usuario
        [JsonPropertyName("name")]              //esto le dice a .NET que si viene name lo guarde aca
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Nombre de la empresa para crear el Tenant
        public string BusinessName { get; set; } = string.Empty;
    }
}