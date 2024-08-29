using CreationsPlatformWebApplication.DataAccess.Contexts;
using CreationsPlatformWebApplication.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public class CreationRepository(ApplicationDbContext dbContext) : ICreationRepository
{
    public async Task<List<CreationEntity?>> GetAll() =>
        await dbContext.Creations.ToListAsync();

    public async Task<CreationEntity?> GetById(int id) =>
        await dbContext
            .Creations
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<CreationEntity?>> GetByAuthorId(Guid id) =>
        await dbContext
            .Creations
            .Where(x => x.AuthorId == id)
            .ToListAsync();


    public async Task Create(CreationEntity entity)
    {
        entity.Author = null!;
        if (entity.Id <= 0)
        {
            entity.Id = dbContext.Creations.OrderBy(e => e.Id).Last().Id + 1;
        }
        entity.PublicationDate = DateTime.SpecifyKind(entity.PublicationDate, DateTimeKind.Utc);
        
        dbContext.Creations.Add(entity);
        await dbContext.SaveChangesAsync();
    }


    public async Task<bool> Delete(int id)
    {
        var entity = await GetById(id);
        if (entity == null) return false;

        dbContext.Creations.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task Update(CreationEntity entity)
    {
        dbContext.Update(entity);
        await dbContext.SaveChangesAsync();
    }
}