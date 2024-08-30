using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.Messages;

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
    Task<(List<CreationEntity> items, int count)> GetPagedSortedFiltered(IndexRequest request);
}