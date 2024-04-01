
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.UserManagement;
using CFusionRestaurant.ViewModel.UserManagement.Request;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.UserControllerTests;

public class InsertUserTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _controller;

    public InsertUserTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UserController(_userServiceMock.Object);
    }

    [Fact]
    public async Task ShouldReturnsCreated()
    {
        // Arrange
        var userInsertRequestViewModel = new UserInsertRequestViewModel
        {
            Name = "name",
            EMail = "email",
            Password = "password",
            IsAdmin = true,
        };
        var userId = ObjectId.GenerateNewId().ToString();

        _userServiceMock.Setup(service => service.InsertAsync(userInsertRequestViewModel))
                        .ReturnsAsync(userId);

        // Act
        var result = await _controller.Insert(userInsertRequestViewModel);

        // Assert
        result.Should().BeOfType<CreatedResult>()
            .Which.Location.Should().Be($"/user/{userId}");

    }

    [Fact]
    public async Task ShouldReturnsBadRequest()
    {
        // Arrange
        var userInsertRequestViewModel = new UserInsertRequestViewModel();
        _controller.ModelState.AddModelError("EMail", "ErrorMessage");

        // Act
        var result = await _controller.Insert(userInsertRequestViewModel);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
