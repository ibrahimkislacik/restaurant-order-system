
using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.OrderManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.OrderManagement;
using CFusionRestaurant.ViewModel.OrderManagement;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.OrderManagement.OrderServiceTests;

public class ListOrderTests
{
    private readonly Mock<IRepository<Order>> _orderRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public ListOrderTests()
    {
        _orderRepositoryMock = new Mock<IRepository<Order>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldReturnListOfOrderViewModel()
    {
        // Arrange step - for setup the testing objects and prepare the prerequisites for test
        var orders = new List<Order>
            {
                new Order { Id = ObjectId.GenerateNewId(), Total = 5 },
                new Order { Id = ObjectId.GenerateNewId(),  Total = 6  }
            };

        _orderRepositoryMock.Setup(repo => repo.ListAsync()).ReturnsAsync(orders);

        _mapperMock.Setup(mapper => mapper.Map<List<OrderViewModel>>(orders)).Returns(new List<OrderViewModel>
            {
                new OrderViewModel { Id = "1", Total = 5 },
                new OrderViewModel { Id = "2", Total = 6 }
            });

        var orderService = new OrderService(_orderRepositoryMock.Object, null, null, _mapperMock.Object);

        // Act step - perform the actual work of the test
        var result = await orderService.ListAsync();

        // Assert step - verify the result
        Assert.NotNull(result);
        Assert.Equal(orders.Count, result.Count);
    }
}
