using AutoMapper;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.Models;
using CreationsPlatformWebApplication.Models.Creation;

namespace CreationsPlatformWebApplication.Profiles;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<GenreModel, GenreEntity>().ReverseMap();
    }
}