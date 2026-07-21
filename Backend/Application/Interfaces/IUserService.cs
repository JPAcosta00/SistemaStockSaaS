using Application.DTOs;

public interface IUserService
{
    Task<bool> UpdateProfileAsync(Guid userId, UpdateUserByAdminDto dto);
}