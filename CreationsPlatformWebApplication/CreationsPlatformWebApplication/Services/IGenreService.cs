using CreationsPlatformWebApplication.Models.Creation;

namespace CreationsPlatformWebApplication.Services;

public interface IGenreService
{
    Task<List<GenreModel>> GetGenres();
}