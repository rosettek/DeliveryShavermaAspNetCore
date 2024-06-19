using Microsoft.EntityFrameworkCore;
using UserService.Core;
using UserService.Core.abstractions;
using UserService.Core.Exceptions;
using UserService.DataAccess.Entities;
using UserService.DataAccess.Extentions;

namespace UserService.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _userDbContext;

    public UserRepository(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }


    public async Task<Guid> Save(MyUser user)
    {
        await _userDbContext.Users.AddAsync(user.ToUserEntity());
        await _userDbContext.SaveChangesAsync();
        return user.UserId;
    }

    public async Task<bool> Update(MyUser user)
    {
        var userEntity = await _userDbContext.Users.FirstOrDefaultAsync(x => x.Id == user.UserId);
        if (userEntity == null) return false;

        userEntity.Comment = user.Comment;
        userEntity.Addresses = user.Addresses.ToAddressEntity(userEntity.Id);
        userEntity.PhoneNumber = user.PhoneNumber;

        await _userDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<MyUser?> Get(Guid id)
    {
        var user = await _userDbContext.Users.AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        user.Addresses =
            _userDbContext.Addresses.AsNoTracking().Where(a => a.UserEntityId == user.Id).ToList();
        return user?.ToModel();
    }
}