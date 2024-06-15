using AuthService.Core;
using AuthService.DataAccess.Entities;

namespace AuthService.DataAccess.Extensions;

public static class CoreExtensions
{
    public static UserAuthEntity ToEntity(this UserAuth user)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        return new UserAuthEntity
        {
            Id = user.Id,
            PasswordHash = user.PasswordHash,
            Email = user.Email
        };
    }

    public static CurierAuthEntity ToEntity(this CurierAuth curier)
    {
        
        ArgumentNullException.ThrowIfNull(curier);
        return new CurierAuthEntity
        {
            Id = curier.Id,
            PasswordHash = curier.PasswordHash,
            Login = curier.Login
        };
    }

    public static StoreAuthEntity ToEntity(this StoreAuth store)
    {
        
        ArgumentNullException.ThrowIfNull(store);

        return new StoreAuthEntity
        {
            Id = store.Id,
            PasswordHash = store.PasswordHash,
            Login = store.Login
        };
    }
}