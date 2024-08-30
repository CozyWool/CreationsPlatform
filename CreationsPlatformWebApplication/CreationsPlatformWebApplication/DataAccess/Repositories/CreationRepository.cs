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
            .Include(entity => entity.Genres)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<CreationEntity?>> GetByAuthorId(Guid id) =>
        await dbContext
            .Creations
            .Where(x => x.AuthorId == id)
            .ToListAsync();


    public async Task Create(CreationEntity entity)
    {
        entity.Author = null!;

        await KostylSManyToMany(entity.Genres);

        if (entity.Id <= 0)
        {
            entity.Id = dbContext.Creations.OrderBy(e => e.Id).Last().Id + 1;
        }

        entity.PublicationDate = DateTime.SpecifyKind(entity.PublicationDate, DateTimeKind.Utc);


        dbContext.Creations.Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(CreationEntity entity)
    {
        await KostylSManyToMany(entity.Genres);

        dbContext.Update(entity);
        await dbContext.SaveChangesAsync();
    }

    //TODO: надо че-то делать с ним
    //TODO: 4 часа убил, все никак не понял как правильно инсертить Many-to-Many сущности, если уже есть запись в таблице
    //TODO: edit не работает с этим костылём ***
    private async Task KostylSManyToMany(ICollection<GenreEntity> list)
    {
        foreach (var genre in list)
        {
            var genreInDb = await dbContext.Genres.FindAsync(genre.Id);
            if (genreInDb != null)
            {
                dbContext.Genres.Remove(genreInDb);
            }
        }
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await GetById(id);
        if (entity == null) return false;

        dbContext.Creations.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<CreationEntity?>> GetUsersCreations(Guid userId) =>
        await dbContext.Creations.Where(entity => entity.AuthorId == userId).ToListAsync();
}