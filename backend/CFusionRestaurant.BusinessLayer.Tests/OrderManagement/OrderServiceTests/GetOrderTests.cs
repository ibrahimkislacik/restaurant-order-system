using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.OrderManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.OrderManagement;
using CFusionRestaurant.ViewModel.OrderManagement;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.OrderManagement.OrderServiceTests;

public class GetOrderTests
{
    private readonly Mock<IRepository<Order>> _orderRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public GetOrderTests()
    {
        _orderRepositoryMock = new Mock<IRepository<Order>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldGetOrderById()
    {
        // Arrange
        var orderId = ObjectId.GenerateNewId();
        var expectedOrder = new Order
        {
            Id = orderId,
            Total = 5,
        };

        _orderRepositoryMock.Setup(repo => repo.GetAsync(orderId.ToString())).ReturnsAsync(expectedOrder);

        var orderViewModel = new OrderViewModel()
        {
            Id = orderId.ToString(),
            Total = 5
        };
        _mapperMock.Setup(mapper => mapper.Map<OrderViewModel>(expectedOrder)).Returns(orderViewModel);

        var orderService = new OrderService(_orderRepositoryMock.Object, null, null, _mapperMock.Object);

        // Act
        var result = await orderService.GetAsync(orderId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedOrder.Id.ToString(), result.Id);
    }

}
