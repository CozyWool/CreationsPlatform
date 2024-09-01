using CreationsPlatformWebApplication.DataAccess.Contexts;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.Enums;
using Microsoft.EntityFrameworkCore;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public class CreationRepository(ApplicationDbContext applicationDbContext) : ICreationRepository
{
    public async Task<List<CreationEntity>> GetAll() =>
        await applicationDbContext
            .Creations
            .Include(entity => entity.Genres)
            .Include(entity => entity.Comments)
            .ToListAsync();

    public async Task<CreationEntity?> GetById(int id) => await applicationDbContext
        .Creations
        .Include(entity => entity.Genres)
        .Include(entity => entity.Comments)
        .AsSplitQuery()
        .FirstOrDefaultAsync(x => x.Id == id);


    public async Task<List<CreationEntity>> GetByAuthorId(Guid id) =>
        await applicationDbContext
            .Creations
            .Include(entity => entity.Genres)
            .Include(entity => entity.Comments)
            .Where(x => x.AuthorId == id)
            .ToListAsync();


    public async Task Create(CreationEntity entity)
    {
        entity.Author = null!;

        var existingGenres = new List<GenreEntity>();
        foreach (var genre in entity.Genres)
        {
            existingGenres.AddRange(applicationDbContext.Genres.Where(e => e.Id == genre.Id));
        }

        entity.Genres = new List<GenreEntity>(existingGenres);

        if (entity.Id <= 0)
        {
            if (await applicationDbContext.Creations.AnyAsync())
                entity.Id = applicationDbContext.Creations.OrderBy(e => e.Id).Last().Id + 1;
            else
                entity.Id = 0;
        }

        entity.PublicationDate = DateTime.SpecifyKind(entity.PublicationDate, DateTimeKind.Utc);


        applicationDbContext.Creations.Add(entity);
        await applicationDbContext.SaveChangesAsync();
    }

    public async Task Update(CreationEntity entity)
    {
        var existingGenres = new List<GenreEntity>();
        foreach (var genre in entity.Genres)
        {
            existingGenres.AddRange(applicationDbContext.Genres.Where(e => e.Id == genre.Id));
        }

        entity.Genres = new List<GenreEntity>(existingGenres);

        applicationDbContext.Update(entity);
        await applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await GetById(id);
        if (entity == null) return false;

        applicationDbContext.Creations.Remove(entity);
        await applicationDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<CreationEntity>> GetUsersCreations(Guid userId) =>
        await applicationDbContext.Creations.Where(entity => entity.AuthorId == userId).ToListAsync();

    public async Task<(List<CreationEntity> items, int count)> GetPagedSortedFiltered(int pageNumber,
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
        //фильтрация
        var creations = applicationDbContext
            .Creations
            .Include(x => x.Genres)
            .Include(x => x.Comments)
            .AsQueryable();

        if (genreId != null && genreId != -1)
        {
            creations = creations.Where(p => p.Genres.Any(genre => genre.Id == genreId));
        }

        if (!string.IsNullOrEmpty(title))
        {
            creations = creations.Where(p =>
                p.Title.ToLower().Trim()
                    .Contains(title.ToLower().Trim()));
        }

        if (!string.IsNullOrEmpty(authorUsername))
        {
            creations = isAuthorUsernameStrict switch
            {
                true => creations.Where(p => p.Author.Username.ToLower().Trim() == authorUsername.ToLower().Trim()),
                _ => creations.Where(p => p.Author.Username.ToLower().Trim().Contains(authorUsername.ToLower().Trim()))
            };
        }

        if (publishedAfter.HasValue)
        {
            publishedAfter = DateTime.SpecifyKind(publishedAfter.Value, DateTimeKind.Utc);
            creations = creations.Where(x => x.PublicationDate.Date >= publishedAfter.Value.Date);
        }

        if (publishedBefore.HasValue)
        {
            publishedBefore = DateTime.SpecifyKind(publishedBefore.Value, DateTimeKind.Utc);
            creations = creations.Where(x => x.PublicationDate.Date <= publishedBefore.Value.Date);
        }

        if (ratingBefore is >= 0 and <= 100)
        {
            creations = creations.Where(x => x.TotalRating <= ratingBefore);
        }

        if (ratingAfter is >= 0 and <= 100)
        {
            creations = creations.Where(x => x.TotalRating >= ratingAfter);
        }

        // сортировка
        creations = sortOrder switch
        {
            SortState.IdAsc => creations.OrderBy(e => e.Id),
            SortState.IdDesc => creations.OrderByDescending(e => e.Id),
            SortState.TitleAsc => creations.OrderBy(e => e.Title),
            SortState.TitleDesc => creations.OrderByDescending(e => e.Title),
            SortState.AuthorUsernameAsc => creations.OrderBy(e => e.Author.Username),
            SortState.AuthorUsernameDesc => creations.OrderByDescending(e => e.Author.Username),
            SortState.PublicationDateAsc => creations.OrderBy(e => e.PublicationDate),
            SortState.PublicationDateDesc => creations.OrderByDescending(e => e.PublicationDate),
            SortState.RatingAsc => creations.OrderBy(e => e.TotalRating),
            SortState.RatingDesc => creations.OrderByDescending(e => e.TotalRating),
            SortState.CommentAsc => creations.OrderBy(e => e.CommentCount),
            SortState.CommentDesc => creations.OrderByDescending(e => e.CommentCount),
            _ => creations.OrderBy(e => e.Id)
        };


        // пагинация
        var count = await creations.CountAsync();
        if (limit != null)
        {
            creations = creations.Take(limit.Value);
            if (limit < count)
            {
                count = limit.Value;
            }
        }

        var items = await creations
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, count);
    }

    public async Task<(CreationEntity entity, int count)> GetByIdPaged(int id, int pageNumber, int pageSize)
    {
        var entity = await GetById(id);

        var contentSplit = entity.Content.Split(" ");
        var count = contentSplit.Length;
        contentSplit = contentSplit
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArray();
        entity.Content = string.Join(" ", contentSplit);
        return (entity, count);
    }
}