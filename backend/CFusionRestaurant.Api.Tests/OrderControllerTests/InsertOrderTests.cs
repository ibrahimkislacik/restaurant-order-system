
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.OrderManagement;
using CFusionRestaurant.ViewModel.OrderManagement.Request;
using CFusionRestaurant.ViewModel.OrderManagement.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.OrderControllerTests;

public class InsertOrderTests
{

    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly OrderController _controller;

    public InsertOrderTests()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _controller = new OrderController(_orderServiceMock.Object);
    }

    [Fact]
    public async Task ShouldReturnsOk()
    {
        // Arrange
        var productId = ObjectId.GenerateNewId();
        var orderInsertRequestViewModel = new OrderInsertRequestViewModel
        {
            OrderProducts = new List<OrderProductRequestViewModel>
            {
            new OrderProductRequestViewModel { ProductId = productId.ToString(), Quantity = 1 }
        }
        };
        var orderInsertResponseViewModel = new OrderInsertResponseViewModel
        {
            OrderNo = "123456"
        };

        _orderServiceMock.Setup(service => service.InsertAsync(orderInsertRequestViewModel))
                         .ReturnsAsync(orderInsertResponseViewModel);

        // Act
        var result = await _controller.Insert(orderInsertRequestViewModel);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(orderInsertResponseViewModel);
    }

    [Fact]
    public async Task ShouldReturnsBadRequest()
    {
        // Arrange
        var orderInsertRequestViewModel = new OrderInsertRequestViewModel();

        _controller.ModelState.AddModelError("OrderProducts", "ErrorMessage");

        // Act
        var result = await _controller.Insert(orderInsertRequestViewModel);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

}
