using CreationsPlatformWebApplication.DataAccess.Contexts;
using CreationsPlatformWebApplication.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreationsPlatformWebApplication.DataAccess.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<List<UserEntity?>> GetAll() =>
        await dbContext.Users.ToListAsync();

    public async Task<UserEntity?> GetById(Guid id) =>
        await dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<UserEntity?> GetByEmail(string email) =>
        await dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Email == email);

    public async Task<UserEntity> GetByLogin(string login) => await dbContext
        .Users
        .FirstOrDefaultAsync(x => x.Login == login);

    public async Task Create(UserEntity? entity)
    {
        dbContext.Users.Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var entity = await GetById(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        await Update(entity);
    }

    public async Task Update(UserEntity entity)
    {
        dbContext.Update(entity);
        await dbContext.SaveChangesAsync();
    }
}