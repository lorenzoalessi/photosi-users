using AutoMapper;
using PhotosiUsers.Dto;
using PhotosiUsers.Model;
using PhotosiUsers.Utility;

namespace PhotosiUsers.Mapper;

public class UserMapperProfile: Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto>()
            // Password hashata
            .ForMember(x => x.Password, y => y.MapFrom(z => z.Password.ConvertToSha512()))
            .ReverseMap();
    }
}