using CreationsPlatformWebApplication.DataAccess.Entities;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public interface ICommentRepository
{
    Task<List<CommentEntity>> GetAll();
    Task<CommentEntity?> GetById(int id);
    // Task<CommentEntity?> GetByCreationId(int creationId);
    Task Create(CommentEntity entity);
    Task<bool> Delete(int id);
}