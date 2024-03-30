
using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.UserManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.UserManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.Settings;
using CFusionRestaurant.ViewModel.UserManagement.Request;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using System.Linq.Expressions;

namespace CFusionRestaurant.BusinessLayer.Tests.UserManagement.UserServiceTests;

public class InsertUserTests
{

    private readonly Mock<IAppSettings> _appSettingsMock;
    private readonly Mock<IRepository<User>> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public InsertUserTests()
    {
        _appSettingsMock = new Mock<IAppSettings>();
        _userRepositoryMock = new Mock<IRepository<User>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldInsertUser()
    {
        // Arrange
        var userInsertRequestViewModel = new UserInsertRequestViewModel
        {
            EMail = "newuser@example.com",
            Password = "password",
            Name = "New User"
        };

        _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                           .ReturnsAsync((User)null);

        var newUser = new User { Id = ObjectId.GenerateNewId(), EMail = userInsertRequestViewModel.EMail };

        _mapperMock.Setup(mapper => mapper.Map<User>(userInsertRequestViewModel)).Returns(newUser);

        var userService = new UserService(_appSettingsMock.Object, _userRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await userService.InsertAsync(userInsertRequestViewModel);

        // Assert
        result.Should().NotBeNull().And.NotBeEmpty().And.Be(newUser.Id.ToString());
    }

    [Fact]
    public async Task ShouldThrowBusinessException_WhenEmailAlreadyExist()
    {
        // Arrange
        var existingUserEmail = "existinguser@example.com";
        var userInsertRequestViewModel = new UserInsertRequestViewModel
        {
            EMail = existingUserEmail,
            Password = "password",
            Name = "Existing User"
        };

        _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                           .ReturnsAsync(new User { EMail = existingUserEmail });

        var userService = new UserService(_appSettingsMock.Object, _userRepositoryMock.Object, _mapperMock.Object);

        // Act
        Func<Task> action = async () => await userService.InsertAsync(userInsertRequestViewModel);

        //Assert
        await action.Should().ThrowAsync<BusinessException>();

    }
}
