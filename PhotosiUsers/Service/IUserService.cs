using PhotosiUsers.Dto;

namespace PhotosiUsers.Service;

public interface IUserService
{
    Task<List<UserDto>> GetAsync();
}