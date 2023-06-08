using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Moq;
using NSubstitute;
using xunit_autofixture.services;
using xunit_autofixture.services.Interfaces;
using xunit_autofixture.services.Models;

namespace xunit_autofixture.tests;

public class UnitTest_With_AutoFixture_NSubtitude
{
    private readonly IFixture _fixture;

    public UnitTest_With_AutoFixture_NSubtitude()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
    }

    [Fact]
    public async Task UserService_CreateUser_Should_work()
    {
        // arrange
        var user = _fixture.Create<UserModel>();

        var dbContext = _fixture.Create<IAppDbContext>();
        var userList = new List<User>();
        dbContext.Users.Returns(StaticDbSet.GetQueryableMockDbSet(userList));
        _fixture.Inject(dbContext);

        // act
        var userService = _fixture.Create<UserService>();
        var result = await userService.CreateUser(user);

        // assert
        await dbContext.Received().SaveChangesAsync(default);
        userList.Count().Should().Be(1);
        result.Age.Should().Be(user.Age);
        result.Name.Should().Be(user.Name);
    }
}