using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using PhotosiUsers.Dto;
using PhotosiUsers.Model;
using PhotosiUsers.Utility;

namespace PhotosiUsers.Mapper;

[ExcludeFromCodeCoverage]
public class UserMapperProfile: Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserDto, User>()
            // Password hashata
            .ForMember(x => x.Password, y => y.MapFrom(z => z.Password.ConvertToSha512()))
            .ReverseMap();
    }
}