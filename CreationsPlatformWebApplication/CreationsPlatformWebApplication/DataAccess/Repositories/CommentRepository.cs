using CreationsPlatformWebApplication.DataAccess.Contexts;
using CreationsPlatformWebApplication.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ICreationRepository _creationRepository;

    public CommentRepository(ApplicationDbContext applicationDbContext, ICreationRepository creationRepository)
    {
        _applicationDbContext = applicationDbContext;
        _creationRepository = creationRepository;
    }

    public async Task<List<CommentEntity>> GetAll() =>
        await _applicationDbContext
            .Comments
            .ToListAsync();

    public async Task<CommentEntity?> GetById(int id) =>
        await _applicationDbContext
            .Comments
            .Include(x => x.User)
            .FirstOrDefaultAsync(entity => entity.Id == id);

    public async Task Create(CommentEntity entity)
    {
        entity.User = null!;

        entity.PublicationDate = DateTime.UtcNow;

        if (entity.Id <= 0)
        {
            if (await _applicationDbContext.Comments.AnyAsync())
                entity.Id = _applicationDbContext.Comments.OrderBy(e => e.Id).Last().Id + 1;
            else
                entity.Id = 0;
        }

        _applicationDbContext.Comments.Add(entity);
        var creationEntity = await _creationRepository.GetById(entity.CreationId);

        var existingCommentEntity = await _applicationDbContext.Comments.FirstOrDefaultAsync(commentEntity =>
            commentEntity.CreationId == entity.CreationId && commentEntity.UserId == entity.UserId);
        if (existingCommentEntity != null && existingCommentEntity.Rating != -1)
        {
            entity.Rating = -1;
        }
        else
        {
            creationEntity.TotalRating = (creationEntity.TotalRating * creationEntity.RatingCount + entity.Rating) /
                                         (creationEntity.RatingCount + 1);
            creationEntity.RatingCount++;
        }

        creationEntity.CommentCount++;

        await _creationRepository.Update(creationEntity);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task Update(CommentEntity entity)
    {
        var oldEntity = await GetById(entity.Id);
        var creationEntity = await _creationRepository.GetById(entity.CreationId);

        // Убрали старую оценку из общей
        creationEntity.TotalRating = (creationEntity.TotalRating * creationEntity.RatingCount - oldEntity.Rating) /
                                     (creationEntity.RatingCount - 1);

        // Добавили новую оценку в общую
        creationEntity.TotalRating = (creationEntity.TotalRating * creationEntity.RatingCount + entity.Rating) /
                                     (creationEntity.RatingCount + 1);

        oldEntity.Rating = entity.Rating;
        oldEntity.PublicationDate = entity.PublicationDate;
        oldEntity.Content = entity.Content;
        _applicationDbContext.Update(oldEntity);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await GetById(id);
        if (entity == null) return false;

        var creationEntity = await _creationRepository.GetById(entity.CreationId);

        if (entity.Rating != -1)
        {
            creationEntity.TotalRating = creationEntity.RatingCount switch
            {
                1 => 0,
                _ => (creationEntity.TotalRating * creationEntity.RatingCount - entity.Rating) /
                     (creationEntity.RatingCount - 1)
            };

            creationEntity.RatingCount--;
        }

        creationEntity.CommentCount--;


        _applicationDbContext.Comments.Remove(entity);
        await _applicationDbContext.SaveChangesAsync();
        return true;
    }
}