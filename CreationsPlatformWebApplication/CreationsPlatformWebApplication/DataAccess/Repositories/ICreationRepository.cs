using CreationsPlatformWebApplication.DataAccess.Entities;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public interface ICreationRepository
{
    Task<List<CreationEntity?>> GetAll();
    Task<CreationEntity?> GetById(int id);
    Task<List<CreationEntity?>> GetByAuthorId(Guid id);
    Task Create(CreationEntity entity);
    Task Update(CreationEntity entity);
    Task<bool> Delete(int id);
    Task<List<CreationEntity?>> GetUsersCreations(Guid userId);
}