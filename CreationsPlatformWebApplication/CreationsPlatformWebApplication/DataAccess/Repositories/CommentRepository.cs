using CreationsPlatformWebApplication.DataAccess.Contexts;
using CreationsPlatformWebApplication.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CommentRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<CommentEntity>> GetAll() =>
        await _applicationDbContext
            .Comments
            .ToListAsync();

    public async Task<CommentEntity?> GetById(int id) =>
        await _applicationDbContext.Comments.FirstOrDefaultAsync(entity => entity.Id == id);

    // public async Task<CommentEntity?> GetByCreationId(int creationId) =>
    //     await _applicationDbContext.Comments.FirstOrDefaultAsync(entity => entity.CreationId == creationId);

    public async Task Create(CommentEntity entity)
    {
        entity.User = null!;

        if (entity.Id <= 0)
        {
            if (await _applicationDbContext.Comments.AnyAsync())
                entity.Id = _applicationDbContext.Comments.OrderBy(e => e.Id).Last().Id + 1;
            else
                entity.Id = 0;
        }

        entity.PublicationDate = DateTime.UtcNow;

        _applicationDbContext.Comments.Add(entity);
        (await _applicationDbContext.Creations.FirstOrDefaultAsync(x => x.Id == entity.CreationId)).CommentCount++;
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await GetById(id);
        if (entity == null) return false;

        _applicationDbContext.Comments.Remove(entity);
        await _applicationDbContext.SaveChangesAsync();
        return true;
    }
}