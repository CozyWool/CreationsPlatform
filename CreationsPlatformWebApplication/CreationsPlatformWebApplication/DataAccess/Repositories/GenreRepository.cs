using CreationsPlatformWebApplication.DataAccess.Contexts;
using CreationsPlatformWebApplication.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public GenreRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<GenreEntity>> GetAll() => await _applicationDbContext.Genres.ToListAsync();

    public async Task<GenreEntity?> GetById(int id) => 
        await _applicationDbContext.Genres.FirstOrDefaultAsync(entity => entity.Id == id);
}