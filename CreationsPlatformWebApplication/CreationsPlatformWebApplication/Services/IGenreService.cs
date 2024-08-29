using CreationsPlatformWebApplication.Models;

namespace CreationsPlatformWebApplication.Services;

public interface IGenreService
{
    Task<List<GenreModel>> GetGenres();
    Task<List<GenreModel>> FillNameField(List<GenreModel> list);
}