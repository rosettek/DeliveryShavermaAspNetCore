﻿using UserService.Core;
using UserService.Core.Abstractions;
using UserService.Core.Exceptions;

namespace UserService.Application;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<MyUser> Get(Guid userId)
    {
        var user = await _userRepository.Get(userId);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        return user;
    }

    public async Task Save(MyUser user)
    {
        await _userRepository.Add(user);
    }

    public async Task<MyUser> AddNewOrUpdate(Guid userId, string address, string phoneNumber, string comment)
    {
        var newUser = CreateUserModel(userId, [address,], phoneNumber, comment);

        var existingUser = await _userRepository.Get(userId);

        if (existingUser == null)
        {
            return await _userRepository.Add(newUser);
        }

        return await UpdateExistingUser(existingUser, newUser);

    }

    private MyUser CreateUserModel(Guid userId, List<string> address, string phoneNumber, string comment)
    {
        var createUserResult = MyUser.Create(userId, address, comment, phoneNumber);
        if (!string.IsNullOrEmpty(createUserResult.error))
        {
            throw new ArgumentException(createUserResult.error);
        }

        return createUserResult.myUser!;
    }

    private async Task<MyUser> UpdateExistingUser(MyUser existingUser, MyUser newUser)
    {
        existingUser.MergeWith(newUser);
        if (await _userRepository.Update(existingUser))
        {
            return existingUser;
        }
        else
        {
            throw new InvalidOperationException("User could not be saved.");
        }
    }

    public async Task<MyUser> Add(Guid userId, List<string> addresses, string phoneNumber, string comment)
    {
        var user = CreateUserModel(userId, addresses, comment, phoneNumber);

        await _userRepository.Add(user);

        return user;
    }
}