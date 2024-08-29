using AutoMapper;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.Models;

namespace CreationsPlatformWebApplication.Profiles;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<UserEntity, AuthorModel>().ReverseMap();
    }
}