using CreationsPlatformWebApplication.DataAccess.Entities;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public interface IUserRepository
{
    Task<List<UserEntity?>> GetAll();
    Task<UserEntity?> GetById(Guid id);
    Task<UserEntity?> GetByEmail(string email);
    Task<UserEntity> GetByLogin(string login);
    Task Create(UserEntity? entity);
    Task Update(UserEntity entity);
    Task Delete(Guid id);
}