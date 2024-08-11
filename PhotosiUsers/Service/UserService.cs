using AutoMapper;
using PhotosiUsers.Dto;
using PhotosiUsers.Repository.User;

namespace PhotosiUsers.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<List<UserDto>> GetAsync()
    {
        var user = await _userRepository.GetAsync();
        return _mapper.Map<List<UserDto>>(user);
    }
}