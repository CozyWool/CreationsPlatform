using AutoMapper;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.DataAccess.Repositories;
using CreationsPlatformWebApplication.Models.Creation;

namespace CreationsPlatformWebApplication.Services;

public class CommentService(ICommentRepository commentRepository, IMapper mapper) : ICommentService
{
    public async Task<List<CommentModel>> GetAll() =>
        mapper.Map<List<CommentModel>>(await commentRepository.GetAll());

    public async Task<CommentModel?> GetById(int id) =>
        mapper.Map<CommentModel?>(await commentRepository.GetById(id));

    public async Task Create(CommentModel model)
    {
        var entity = mapper.Map<CommentEntity>(model);
        await commentRepository.Create(entity);
    }

    public async Task<bool> Delete(int id) =>
        await commentRepository.Delete(id);
}