
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

        _mapperMock.Setup(mapper => mapper.Map<User>(userInsertRequestViewModel))
                   .Returns(new User { Id = ObjectId.GenerateNewId(), EMail = userInsertRequestViewModel.EMail });

        var userService = new UserService(_appSettingsMock.Object, _userRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await userService.InsertAsync(userInsertRequestViewModel);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
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

        // Act and Assert
        await Assert.ThrowsAsync<BusinessException>(() => userService.InsertAsync(userInsertRequestViewModel));
    }
}
