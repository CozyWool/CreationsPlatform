using CreationsPlatformWebApplication.Models.Creation;

namespace CreationsPlatformWebApplication.Services;

public interface ICommentService
{
    Task<List<CommentModel>> GetAll();
    Task<CommentModel?> GetById(int id);
    Task Create(CommentModel model);
    Task<bool> Delete(int id);
}