using AutoMapper;
using PhotosiUsers.Dto;
using PhotosiUsers.Model;

namespace PhotosiUsers.Mapper;

public class UserMapperProfile: Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto>()
            .ReverseMap();
    }
}