using PhotosiUsers.Dto;

namespace PhotosiUsers.Service;

public interface IUserService
{
    Task<List<UserDto>> GetAsync();

    Task<UserDto> GetByIdAsync(int id);
    
    Task<UserDto> UpdateAsync(int id, UserDto userDto);
    
    Task<UserDto> AddAsync(UserDto userDto);

    Task<UserDto> LoginAsync(LoginDto loginDto);
    
    Task<bool> DeleteAsync(int id);
}