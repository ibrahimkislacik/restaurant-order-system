
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.OrderManagement;
using CFusionRestaurant.ViewModel.OrderManagement;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.OrderControllerTests;

public class GetOrderTests
{
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly OrderController _controller;

    public GetOrderTests()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _controller = new OrderController(_orderServiceMock.Object);
    }

    [Fact]
    public async Task ShouldReturnsOrder()
    {
        // Arrange
        var orderId = ObjectId.GenerateNewId().ToString();
        var expectedOrder = new OrderViewModel { Id = orderId, Total = 5 };
        _orderServiceMock.Setup(service => service.GetAsync(orderId)).ReturnsAsync(expectedOrder);

        // Act
        var result = await _controller.GetById(orderId);

        // Assert
        var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okObjectResult.Value.Should().BeEquivalentTo(expectedOrder);
    }

    [Fact]
    public async Task ShouldReturnsNotFound()
    {
        // Arrange
        var orderId = ObjectId.GenerateNewId().ToString();
        _orderServiceMock.Setup(service => service.GetAsync(orderId)).ReturnsAsync((OrderViewModel)null);

        // Act
        var result = await _controller.GetById(orderId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>()
              .Which.Value.Should().Be($"Order with Id = {orderId} not found");
    }
}
