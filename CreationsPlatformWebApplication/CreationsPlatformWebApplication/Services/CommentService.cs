using AutoMapper;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.DataAccess.Repositories;
using CreationsPlatformWebApplication.Models.Creation;

namespace CreationsPlatformWebApplication.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public CommentService(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<List<CommentModel>> GetAll() =>
        _mapper.Map<List<CommentModel>>(await _commentRepository.GetAll());

    public async Task<CommentModel?> GetById(int id) =>
        _mapper.Map<CommentModel?>(await _commentRepository.GetById(id));

    public async Task Create(CommentModel model)
    {
        var entity = _mapper.Map<CommentEntity>(model);
        await _commentRepository.Create(entity);
    }

    public async Task<bool> Delete(int id) =>
        await _commentRepository.Delete(id);
}