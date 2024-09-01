using CreationsPlatformWebApplication.DataAccess.Contexts;
using CreationsPlatformWebApplication.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public class GenreRepository(ApplicationDbContext applicationDbContext) : IGenreRepository
{
    public async Task<List<GenreEntity>> GetAll() => await applicationDbContext.Genres.ToListAsync();

    public async Task<GenreEntity?> GetById(int id) => 
        await applicationDbContext.Genres.FirstOrDefaultAsync(entity => entity.Id == id);
}