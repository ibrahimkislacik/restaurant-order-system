using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.OrderManagement;
using CFusionRestaurant.ViewModel.OrderManagement;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.OrderControllerTests;

public class ListOrderTests
{
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly OrderController _controller;

    public ListOrderTests()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _controller = new OrderController(_orderServiceMock.Object);
    }

    [Fact]
    public async Task ShouldReturnsListOfOrders()
    {
        // Arrange
        var orders = new List<OrderViewModel>
        {
            new OrderViewModel { Id = ObjectId.GenerateNewId().ToString(), Total = 100 },
            new OrderViewModel { Id = ObjectId.GenerateNewId().ToString(), Total = 150 }
        };

        _orderServiceMock.Setup(service => service.ListAsync()).ReturnsAsync(orders);

        // Act
        var result = await _controller.List();

        // Assert
        var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okObjectResult.Value.Should().BeAssignableTo<List<OrderViewModel>>();

        var returnedOrders = (List<OrderViewModel>)okObjectResult.Value;
        returnedOrders.Should().HaveCount(2);
        returnedOrders![0].Total.Should().Be(100);
        returnedOrders[1].Total.Should().Be(150);
    }
}
