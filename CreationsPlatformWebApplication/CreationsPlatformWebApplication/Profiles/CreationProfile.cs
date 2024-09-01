using AutoMapper;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.Models.Creation;
using CreationsPlatformWebApplication.Services;

namespace CreationsPlatformWebApplication.Profiles;

public class CreationProfile : Profile
{
    public CreationProfile()
    {

        CreateMap<CreationModel, CreationEntity>();
        CreateMap<CreationEntity, CreationModel>().AfterMap<CreationModelMappingAction>();
    }

    private class CreationModelMappingAction(IUserService userService) : IMappingAction<CreationEntity, CreationModel>
    {
        public void Process(CreationEntity source, CreationModel destination, ResolutionContext context)
        {
            destination.Author = userService.GetAuthorById(source.AuthorId).Result;
        }
    }
}