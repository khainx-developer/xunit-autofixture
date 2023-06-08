using FluentAssertions;
using NSubstitute;
using xunit_autofixture.services;
using xunit_autofixture.services.Interfaces;
using xunit_autofixture.services.Models;

namespace xunit_autofixture.tests;

public class UnitTest_With_NSubtitude
{
    [Fact]
    public async Task UserService_CreateUser_Should_work()
    {
        // arrange
        var user = new UserModel
        {
            Name = "John",
            Age = 10
        };
        var dbContext = Substitute.For<IAppDbContext>();
        var userList = new List<User>();
        dbContext.Users.Returns(StaticDbSet.GetQueryableMockDbSet(userList));

        var helper = Substitute.For<IHelper>();

        // act
        var userService = new UserService(dbContext, helper);
        var result = await userService.CreateUser(user);

        // assert
        await dbContext.Received().SaveChangesAsync(default);
        userList.Count.Should().Be(1);
        result.Age.Should().Be(user.Age);
        result.Name.Should().Be(user.Name);
    }
}