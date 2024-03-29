using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.UserManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.UserManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.Settings;
using CFusionRestaurant.ViewModel.UserManagement.Request;
using MongoDB.Bson;
using Moq;
using System.Linq.Expressions;

namespace CFusionRestaurant.BusinessLayer.Tests.UserManagement.UserServiceTests;

public class LoginTests
{
    private readonly Mock<IAppSettings> _appSettingsMock;
    private readonly Mock<IRepository<User>> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public LoginTests()
    {
        _appSettingsMock = new Mock<IAppSettings>();
        _userRepositoryMock = new Mock<IRepository<User>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldReturnAccessToken_WhenValidCredentialsProvided()
    {
        // Arrange
        var userLoginRequestViewModel = new UserLoginRequestViewModel
        {
            EMail = "test@example.com",
            Password = "password"
        };

        var user = new User
        {
            Id = ObjectId.GenerateNewId(),
            EMail = userLoginRequestViewModel.EMail,
            Password = userLoginRequestViewModel.Password,
            Name = "Test User",
            IsAdmin = false
        };

        _appSettingsMock.SetupGet(u => u.AccessTokenExpireInMinutes).Returns("60");
        _appSettingsMock.SetupGet(u => u.SecretKey).Returns("A1251489B7899C45D45C45F64C4D645A");

        _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                           .ReturnsAsync(user);

        var userService = new UserService(_appSettingsMock.Object, _userRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await userService.LoginAsync(userLoginRequestViewModel);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.True(result.AccessTokenExpireDate > DateTime.UtcNow);
    }

    [Fact]
    public async Task ShouldThrowBusinessException_WhenInvalidCredentialsProvided()
    {
        // Arrange
        var userLoginRequestViewModel = new UserLoginRequestViewModel
        {
            EMail = "test@example.com",
            Password = "wrong"
        };

        _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                           .ReturnsAsync((User)null);

        var userService = new UserService(_appSettingsMock.Object, _userRepositoryMock.Object, _mapperMock.Object);

        // Act and Assert
        await Assert.ThrowsAsync<BusinessException>(() => userService.LoginAsync(userLoginRequestViewModel));
    }


}
