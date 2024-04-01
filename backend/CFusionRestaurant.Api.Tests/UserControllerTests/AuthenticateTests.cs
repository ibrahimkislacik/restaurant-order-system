
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.UserManagement;
using CFusionRestaurant.ViewModel.UserManagement.Request;
using CFusionRestaurant.ViewModel.UserManagement.Response;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CFusionRestaurant.Api.Tests.UserControllerTests;

public class AuthenticateTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _controller;

    public AuthenticateTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UserController(_userServiceMock.Object);
    }

    [Fact]
    public async Task ShouldReturnsToken()
    {
        // Arrange
        var userLoginRequestViewModel = new UserLoginRequestViewModel
        {
            EMail = "test@example.com",
            Password = "password"
        };
        var expectedToken = "mocked_token";
        var expectedExpireDate = DateTime.UtcNow.AddMinutes(30); 

        _userServiceMock.Setup(service => service.LoginAsync(userLoginRequestViewModel))
                        .ReturnsAsync(new UserLoginResponseViewModel
                        {
                            AccessToken = expectedToken,
                            AccessTokenExpireDate = expectedExpireDate
                        });

        // Act
        var result = await _controller.Authenticate(userLoginRequestViewModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var loginResponse = Assert.IsType<UserLoginResponseViewModel>(okResult.Value);
        Assert.Equal(expectedToken, loginResponse.AccessToken);
        Assert.Equal(expectedExpireDate, loginResponse.AccessTokenExpireDate);
    }
}
