using AutoMapper;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.DataAccess.Repositories;
using CreationsPlatformWebApplication.Models;

namespace CreationsPlatformWebApplication.Services;

public class CreationService(ICreationRepository creationRepository, IMapper mapper) : ICreationService
{
    public async Task<List<CreationModel?>> GetAll() =>
        mapper.Map<List<CreationModel?>>(await creationRepository.GetAll());

    public async Task<CreationModel?> GetById(int id) =>
        mapper.Map<CreationModel?>(await creationRepository.GetById(id));


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
        // oldEntity.Genres = model.Genres;
        oldEntity.PublicationDate = model.PublicationDate;
        oldEntity.Rating = model.Rating;
        oldEntity.RatingCount = model.RatingCount;
        await creationRepository.Update(oldEntity);
    }

    public async Task<bool> Delete(int id) => await creationRepository.Delete(id);
}