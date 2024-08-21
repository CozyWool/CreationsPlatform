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

    public async Task<UserEntity?> GetByEmailOrUsername(string email = null, string username = null)
    {
        if (email == null && username == null)
            return null;

        return await dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Username == username || x.Email == email);
    }

    public async Task<UserEntity> GetByUsername(string username) => await dbContext
        .Users
        .FirstOrDefaultAsync(x => x.Username == username);

    public async Task Create(UserEntity? entity)
    {
        dbContext.Users.Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await GetById(id);
        if (entity == null) return false;
        entity.IsDeleted = true;
        await Update(entity);
        return true;
    }

    public async Task<bool> Delete(string username)
    {
        var entity = await GetByUsername(username);
        if (entity == null) return false;
        entity.IsDeleted = true;
        await Update(entity);
        return true;
    }

    public async Task Update(UserEntity entity)
    {
        dbContext.Update(entity);
        await dbContext.SaveChangesAsync();
    }
}