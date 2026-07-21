namespace Application.DTOs
{
    public class UpdateUserByAdminDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
        public Guid TenantId { get; set; } // Permite moverlo de empresa si es necesario
    }
}