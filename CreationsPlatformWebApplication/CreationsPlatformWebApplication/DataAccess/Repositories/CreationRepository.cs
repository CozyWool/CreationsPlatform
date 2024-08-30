using CreationsPlatformWebApplication.DataAccess.Contexts;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.Enums;
using CreationsPlatformWebApplication.Messages;
using Humanizer;
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

    public async Task<(List<CreationEntity> items, int count)> GetPagedSortedFiltered(IndexRequest request)
    {
        //фильтрация
        var creations = dbContext
            .Creations
            .Include(x => x.Genres)
            .AsQueryable();

        if (request.GenreId != null && request.GenreId != -1)
        {
            creations = creations.Where(p => p.Genres.Any(genre => genre.Id == request.GenreId));
        }

        if (!string.IsNullOrEmpty(request.Title))
        {
            creations = creations.Where(p =>
                p.Title.ToLower().Trim()
                    .Contains(request.Title.ToLower().Trim()));
        }

        if (!string.IsNullOrEmpty(request.AuthorUsername))
        {
            creations = creations.Where(p =>
                p.Author.Username.ToLower().Trim()
                    .Contains(request.AuthorUsername.ToLower().Trim()));
        }

        if (request.PublishedAfter.HasValue)
        {
            request.PublishedAfter = DateTime.SpecifyKind(request.PublishedAfter.Value, DateTimeKind.Utc);
            creations = creations.Where(x => x.PublicationDate.Date >= request.PublishedAfter.Value.Date);
        }

        if (request.PublishedBefore.HasValue)
        {
            request.PublishedBefore = DateTime.SpecifyKind(request.PublishedBefore.Value, DateTimeKind.Utc);
            creations = creations.Where(x => x.PublicationDate.Date <= request.PublishedBefore.Value.Date);
        }

        // сортировка
        creations = request.SortOrder switch
        {
            SortState.IdAsc => creations.OrderBy(e => e.Id),
            SortState.IdDesc => creations.OrderByDescending(e => e.Id),
            SortState.TitleAsc => creations.OrderBy(e => e.Title),
            SortState.TitleDesc => creations.OrderByDescending(e => e.Title),
            SortState.AuthorUsernameAsc => creations.OrderBy(e => e.Author.Username),
            SortState.AuthorUsernameDesc => creations.OrderByDescending(e => e.Author.Username),
            SortState.PublicationDateAsc => creations.OrderBy(e => e.PublicationDate),
            SortState.PublicationDateDesc => creations.OrderByDescending(e => e.PublicationDate),
            SortState.RatingAsc => creations.OrderBy(e => e.Rating),
            SortState.RatingDesc => creations.OrderByDescending(e => e.Rating),
            _ => creations.OrderBy(e => e.Id)
        };

        // пагинация
        var count = await creations.CountAsync();
        var items = await creations
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return (items, count);
    }
}