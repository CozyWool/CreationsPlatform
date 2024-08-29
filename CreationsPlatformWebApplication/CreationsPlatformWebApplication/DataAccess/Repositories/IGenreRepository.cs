using CreationsPlatformWebApplication.DataAccess.Entities;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public interface IGenreRepository
{
    Task<List<GenreEntity>> GetAll();
    Task<GenreEntity> GetById(int id);
}