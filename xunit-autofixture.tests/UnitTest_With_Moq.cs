using FluentAssertions;
using Moq;
using xunit_autofixture.services;
using xunit_autofixture.services.Interfaces;
using xunit_autofixture.services.Models;

namespace xunit_autofixture.tests;

public class UnitTest_With_Moq
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
        var dbContext = new Mock<IAppDbContext>();
        var userList = new List<User>();
        dbContext.Setup(i => i.Users).Returns(StaticDbSet.GetQueryableMockDbSet(userList));
        var helper = new Mock<IHelper>();

        // act
        var userService = new UserService(dbContext.Object, helper.Object);
        var result = await userService.CreateUser(user);

        // assert
        dbContext.Verify(i => i.SaveChangesAsync(default), Times.Once);
        userList.Count().Should().Be(1);
        result.Age.Should().Be(user.Age);
        result.Name.Should().Be(user.Name);
    }
}