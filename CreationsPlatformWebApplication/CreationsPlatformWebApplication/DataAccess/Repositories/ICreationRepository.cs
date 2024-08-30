using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.Enums;

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
    Task<(List<CreationEntity> items, int count)> GetPagedSortedFiltered(int pageNumber,
        int pageSize,
        SortState sortOrder,
        int? genreId = null,
        string? title = null,
        string? authorUsername = null,
        DateTime? publishedBefore = null,
        DateTime? publishedAfter = null,
        int? limit = null);
}