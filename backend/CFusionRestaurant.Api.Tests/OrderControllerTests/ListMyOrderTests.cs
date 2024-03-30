using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.OrderManagement;
using CFusionRestaurant.BusinessLayer.Abstract.UserManagement;
using CFusionRestaurant.ViewModel.OrderManagement;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.OrderControllerTests;

public class ListMyOrderTests
{
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly OrderController _controller;

    public ListMyOrderTests()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _controller = new OrderController(_orderServiceMock.Object);
    }

    [Fact]
    public async Task ShouldReturnsListOfOrders()
    {
        // Arrange
        var userId = ObjectId.GenerateNewId().ToString();
        var orders = new List<OrderViewModel>() {
            new OrderViewModel { Id = ObjectId.GenerateNewId().ToString(), OrderUserInfo = new OrderUserInfoViewModel { UserId = userId } },
            new OrderViewModel { Id = ObjectId.GenerateNewId().ToString(), OrderUserInfo = new OrderUserInfoViewModel { UserId = userId } }
        };
        _currentUserServiceMock.Setup(c => c.UserId).Returns(userId);
        _orderServiceMock.Setup(service => service.ListForUserAsync()).ReturnsAsync(orders);

        // Act
        var result = await _controller.MyOrders();

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(orders);
    }

    [Fact]
    public async Task ShouldReturnsEmptyList()
    {
        // Arrange
        var userId = ObjectId.GenerateNewId().ToString();
        var orders = new List<OrderViewModel>();
        _currentUserServiceMock.Setup(c => c.UserId).Returns(userId);
        _orderServiceMock.Setup(service => service.ListForUserAsync()).ReturnsAsync(orders);

        // Act
        var result = await _controller.MyOrders();

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(orders);
    }

}
