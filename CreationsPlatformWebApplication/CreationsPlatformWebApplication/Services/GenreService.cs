using AutoMapper;
using CreationsPlatformWebApplication.DataAccess.Repositories;
using CreationsPlatformWebApplication.Models.Creation;

namespace CreationsPlatformWebApplication.Services;

public class GenreService(IGenreRepository genreRepository, IMapper mapper) : IGenreService
{
    public async Task<List<GenreModel>> GetGenres() =>
        mapper.Map<List<GenreModel>>(await genreRepository.GetAll());

}