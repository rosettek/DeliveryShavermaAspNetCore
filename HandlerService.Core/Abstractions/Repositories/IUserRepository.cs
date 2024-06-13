namespace Handler.Core.Abstractions;

public interface IUserRepository
{
    Task<MyUser?> Get(Guid userId);
    Task<string?> SaveByUserId(MyUser user);
}