using xunit_autofixture.services.Models;

namespace xunit_autofixture.services.Interfaces;

public interface IUserService
{
    Task<UserModel> CreateUser(UserModel user);
}