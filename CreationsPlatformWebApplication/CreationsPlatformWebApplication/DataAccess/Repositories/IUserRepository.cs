using CreationsPlatformWebApplication.DataAccess.Entities;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public interface IUserRepository
{
    Task<List<UserEntity?>> GetAll();
    Task<UserEntity?> GetById(Guid id);
    Task<UserEntity?> GetByEmail(string email);
    Task<UserEntity?> GetByEmailOrUsername(string email, string username);
    Task<UserEntity> GetByUsername(string username);
    Task Create(UserEntity? entity);
    Task Update(UserEntity entity);
    Task<bool> Delete(Guid id);
    Task<bool> Delete(string username);
}