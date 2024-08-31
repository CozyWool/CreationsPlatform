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
    
    private class CommentModelMappingAction : IMappingAction<CommentEntity, CommentModel>
    {
        private readonly IUserService _userService;
        private readonly ICreationService _creationService;

        public CommentModelMappingAction (IUserService userService, ICreationService creationService)
        {
            _userService = userService;
            _creationService = creationService;
        }

        public void Process(CommentEntity source, CommentModel destination, ResolutionContext context)
        {
            destination.Username = _userService.GetAuthorById(source.UserId).Result.Username;
        }
    }
}