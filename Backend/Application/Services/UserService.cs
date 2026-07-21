using Application.DTOs;
using Domain.Interfaces;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateUserByAdminDto dto)
    {
        // 1. Buscamos al usuario por su ID (usando el método genérico heredado)
        var user = await _userRepository.GetByIdAsync(userId); 
        if (user == null) return false;

        // 2. Validamos si el email ya existe en la BD global
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null && existingUser.Id != userId)
        {
            throw new Exception("El correo electrónico ya se encuentra registrado por otro usuario.");
        }

        // 3. Actualizamos los datos
        user.updateDatos(dto.Username, dto.Role, dto.IsActive, dto.Email, dto.TenantId);

        // 4. Guardamos los cambios
         _userRepository.Update(user); 
         await _userRepository.SaveChangesAsync();
        return true; 
    }
}