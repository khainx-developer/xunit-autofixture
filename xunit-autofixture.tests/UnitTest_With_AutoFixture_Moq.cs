using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using xunit_autofixture.services;
using xunit_autofixture.services.Interfaces;
using xunit_autofixture.services.Models;

namespace xunit_autofixture.tests;

public class UnitTest_With_AutoFixture_Moq
{
    private readonly IFixture _fixture;

    public UnitTest_With_AutoFixture_Moq()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
    }

    [Fact]
    public async Task UserService_CreateUser_NameValidation_Should_fail()
    {
        // arrange
        var user = _fixture.Build<UserModel>()
            .With(i => i.Name, "")
            .Create();

        // act
        var userService = _fixture.Create<UserService>();
        var exception = await Assert.ThrowsAsync<Exception>(() => userService.CreateUser(user));

        // assert
        exception.Message.Should().Be("Name is required");
    }

    [Fact]
    public async Task UserService_CreateUser_AgeValidation_Should_fail()
    {
        // arrange
        var user = _fixture.Build<UserModel>()
            .With(i => i.Age, 0)
            .Create();

        // act
        var userService = _fixture.Create<UserService>();
        var exception = await Assert.ThrowsAsync<Exception>(() => userService.CreateUser(user));

        // assert
        exception.Message.Should().Be("Age is required");
    }

    [Fact]
    public async Task UserService_CreateUser_Should_work()
    {
        // arrange
        var user = _fixture.Create<UserModel>();

        var dbContext = _fixture.Create<Mock<IAppDbContext>>();
        var userList = new List<User>();
        dbContext.Setup(i => i.Users).Returns(StaticDbSet.GetQueryableMockDbSet(userList));
        _fixture.Inject(dbContext);

        // act
        var userService = _fixture.Create<UserService>();
        var result = await userService.CreateUser(user);

        // assert
        dbContext.Verify(i => i.SaveChangesAsync(default), Times.Once);
        userList.Count.Should().Be(1);
        result.Age.Should().Be(user.Age);
        result.Name.Should().Be(user.Name);
    }

    [Fact]
    public async Task UserService_UpdateUser_Should_work()
    {
        // arrange
        var userId = Guid.NewGuid();
        var newName = "new name";
        var user = _fixture.Build<UserModel>()
            .With(i => i.Id, userId)
            .With(i => i.Name, newName)
            .Create();

        var dbContext = _fixture.Create<Mock<IAppDbContext>>();
        var userList = new List<User>
        {
            _fixture.Build<User>()
                .With(i => i.Id, userId)
                .With(i => i.Name, "old name")
                .Create()
        };
        dbContext.Setup(i => i.Users).Returns(StaticDbSet.GetQueryableMockDbSet(userList));
        _fixture.Inject(dbContext);

        // act
        var userService = _fixture.Create<UserService>();
        var result = await userService.UpdateUser(user);

        // assert
        dbContext.Verify(i => i.SaveChangesAsync(default), Times.Once);
        result.Age.Should().Be(user.Age);
        result.Name.Should().Be(newName);
    }
}