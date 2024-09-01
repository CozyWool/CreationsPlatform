using AutoMapper;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.Models.Creation;
using CreationsPlatformWebApplication.Services;

namespace CreationsPlatformWebApplication.Profiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CommentModel, CommentEntity>();
        CreateMap<CommentEntity, CommentModel>().AfterMap<CommentModelMappingAction>();
    }
    
    private class CommentModelMappingAction(IUserService userService, ICreationService creationService)
        : IMappingAction<CommentEntity, CommentModel>
    {
        private readonly ICreationService _creationService = creationService;

        public void Process(CommentEntity source, CommentModel destination, ResolutionContext context)
        {
            destination.Username = userService.GetAuthorById(source.UserId).Result.Username;
        }
    }
}