using CreationsPlatformWebApplication.Controllers;
using CreationsPlatformWebApplication.Messages;
using CreationsPlatformWebApplication.Models;
using CreationsPlatformWebApplication.Models.Creation;

namespace CreationsPlatformWebApplication.Services;

public interface ICreationService
{
    Task<List<CreationModel?>> GetAll();
    Task<CreationModel?> GetById(int id);
    Task<List<CreationModel?>> GetByAuthorId(Guid id);
    Task Create(CreationModel model);
    Task Update(CreationModel model);
    Task<bool> Delete(int id);
    Task<List<CreationModel?>> GetUsersCreations(Guid userId);
    Task<(List<CreationModel>, int)> GetPagedSortedFiltered(IndexRequest request);
}