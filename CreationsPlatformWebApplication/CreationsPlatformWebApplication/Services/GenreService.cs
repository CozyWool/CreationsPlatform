using AutoMapper;
using CreationsPlatformWebApplication.DataAccess.Repositories;
using CreationsPlatformWebApplication.Models.Creation;

namespace CreationsPlatformWebApplication.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IMapper _mapper;

    public GenreService(IGenreRepository genreRepository, IMapper mapper)
    {
        _genreRepository = genreRepository;
        _mapper = mapper;
    }

    public async Task<List<GenreModel>> GetGenres() =>
        _mapper.Map<List<GenreModel>>(await _genreRepository.GetAll());

    public async Task<List<GenreModel>> FillNameField(List<GenreModel> list)
    {
        foreach (var item in list)
        {
            item.Name = (await _genreRepository.GetById(item.Id)).Name;
        }
        return list;
    }
}