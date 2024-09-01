using CreationsPlatformWebApplication.DataAccess.Entities;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public interface ICommentRepository
{
    Task<List<CommentEntity>> GetAll();
    Task<CommentEntity?> GetById(int id);
    Task Create(CommentEntity entity);
    Task Update(CommentEntity entity);
    Task<bool> Delete(int id);
}