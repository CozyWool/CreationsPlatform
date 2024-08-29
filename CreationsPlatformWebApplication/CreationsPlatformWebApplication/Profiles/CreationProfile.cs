using System.Reflection;
using AutoMapper;
using AutoMapper.Internal;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.Models;
using CreationsPlatformWebApplication.Services;

namespace CreationsPlatformWebApplication.Profiles;

public class CreationProfile : Profile
{
    public CreationProfile()
    {

        CreateMap<CreationModel, CreationEntity>();
        CreateMap<CreationEntity, CreationModel>().AfterMap<CreationModelMappingAction>();
    }

    private class CreationModelMappingAction : IMappingAction<CreationEntity, CreationModel>
    {
        private readonly IUserService _userService;

        public CreationModelMappingAction(IUserService userService)
        {
            _userService = userService;
        }

        public void Process(CreationEntity source, CreationModel destination, ResolutionContext context)
        {
            destination.Author = _userService.GetAuthorById(source.AuthorId).Result;
        }
    }
}