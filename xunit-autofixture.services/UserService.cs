using xunit_autofixture.services.Interfaces;
using xunit_autofixture.services.Models;

namespace xunit_autofixture.services;

public class UserService : IUserService
{
    private readonly IAppDbContext _dbContext;
    private readonly IHelper _helper;

    public UserService(IAppDbContext dbContext, IHelper helper)
    {
        _dbContext = dbContext;
        _helper = helper;
    }

    public async Task<UserModel> CreateUser(UserModel user)
    {
        // validate user data
        if (string.IsNullOrEmpty(user.Name))
        {
            throw new Exception("Name is required");
        }

        if (user.Age <= 0)
        {
            throw new Exception("Age is required");
        }

        // insert user to database
        var newUser = new User
        {
            Age = user.Age,
            Name = user.Name
        };
        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();

        return new UserModel
        {
            Id = newUser.Id,
            Age = newUser.Age,
            Name = newUser.Name
        };
    }

    public async Task<UserModel> UpdateUser(UserModel user)
    {
        // validate user data
        if (string.IsNullOrEmpty(user.Name))
        {
            throw new Exception("Name is required");
        }

        if (user.Age <= 0)
        {
            throw new Exception("Age is required");
        }

        // get user from database
        var userToSave = _dbContext.Users.FirstOrDefault(i => i.Id == user.Id);
        if (userToSave == null)
        {
            throw new Exception("User is not found");
        }

        userToSave.Age = user.Age;
        userToSave.Name = user.Name;
        await _dbContext.SaveChangesAsync();

        return new UserModel
        {
            Id = userToSave.Id,
            Age = userToSave.Age,
            Name = userToSave.Name
        };
    }
}