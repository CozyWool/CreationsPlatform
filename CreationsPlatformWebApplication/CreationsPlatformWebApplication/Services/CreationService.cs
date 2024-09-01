using AutoMapper;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.DataAccess.Repositories;
using CreationsPlatformWebApplication.Enums;
using CreationsPlatformWebApplication.Models.Creation;

namespace CreationsPlatformWebApplication.Services;

public class CreationService(ICreationRepository creationRepository, IMapper mapper) : ICreationService
{
    public async Task<List<CreationModel>> GetAll() =>
        mapper.Map<List<CreationModel>>(await creationRepository.GetAll());

    public async Task<CreationModel> GetById(int id) =>
        mapper.Map<CreationModel>(await creationRepository.GetById(id));


    public async Task<List<CreationModel?>> GetByAuthorId(Guid id) =>
        mapper.Map<List<CreationModel?>>(await creationRepository.GetByAuthorId(id));

    public async Task Create(CreationModel model)
    {
        var entity = mapper.Map<CreationEntity>(model);
        await creationRepository.Create(entity);
    }

    public async Task Update(CreationModel model)
    {
        var oldEntity = await creationRepository.GetById(model.Id);
        if (oldEntity == null) return;
        oldEntity.Title = model.Title;
        oldEntity.Genres = mapper.Map<List<GenreEntity>>(model.Genres);
        oldEntity.TotalRating = model.TotalRating;
        oldEntity.RatingCount = model.RatingCount;
        oldEntity.Content = model.Content;
        await creationRepository.Update(oldEntity);
    }

    public async Task<bool> Delete(int id) => await creationRepository.Delete(id);

    public async Task<List<CreationModel>> GetUsersCreations(Guid userId) =>
        mapper.Map<List<CreationModel>>(await creationRepository.GetUsersCreations(userId));

    public async Task<(List<CreationModel>, int)> GetPagedSortedFiltered(int pageNumber,
        int pageSize,
        SortState sortOrder,
        int? genreId = null,
        string? title = null,
        string? authorUsername = null,
        bool? isAuthorUsernameStrict = null,
        DateTime? publishedBefore = null,
        DateTime? publishedAfter = null,
        int? ratingBefore = null,
        int? ratingAfter = null,
        int? limit = null)
    {
        var (items, count) = await creationRepository.GetPagedSortedFiltered(pageNumber,
            pageSize,
            sortOrder,
            genreId,
            title,
            authorUsername,
            isAuthorUsernameStrict,
            publishedBefore,
            publishedAfter,
            ratingBefore,
            ratingAfter,
            limit);
        var mappedItems = mapper.Map<List<CreationModel>>(items);
        return (mappedItems, count);
    }

    public async Task<(CreationModel? creationModel, int count)> GetByIdPaged(int id, int pageSize, int pageNumber)
    {
        var (entity, count) = await creationRepository.GetByIdPaged(id, pageNumber, pageSize);
        var model = mapper.Map<CreationModel>(entity);
        return (model, count);
    }
}