using AutoMapper;
using PhotosiUsers.Dto;
using PhotosiUsers.Exceptions;
using PhotosiUsers.Model;
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

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateAsync(int id, UserDto userDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new UserException($"L'utente con ID {id} non esiste");

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Username = userDto.Username;
        user.Email = userDto.Email;
        user.BirthDate = userDto.BirthDate;

        await _userRepository.SaveAsync();

        return userDto;
    }

    public async Task<UserDto> AddAsync(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        await _userRepository.AddAsync(user);
        
        // Aggiorno l'Id della dto senza rimappare
        userDto.Id = user.Id;
        return userDto;
    }

    public async Task<UserDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
        // Se l'utente non esiste per lo username passato oppure la password è errata
        // lancio un'eccezione con un messaggio generico
        if (user == null || user.Password != loginDto.Password)
            throw new UserException("Username o password errate");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> DeleteAsync(int id) => await _userRepository.DeleteAsync(id);
}