
using AutoMapper;
using CFusionRestaurant.BusinessLayer.Abstract.UserManagement;
using CFusionRestaurant.BusinessLayer.Concrete.OrderManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.OrderManagement;
using CFusionRestaurant.ViewModel.OrderManagement;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.OrderManagement.OrderServiceTests;

public class ListForUserOrderTests
{
    private readonly Mock<IRepository<Order>> _orderRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IMapper> _mapperMock;

    public ListForUserOrderTests()
    {
        _orderRepositoryMock = new Mock<IRepository<Order>>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldReturnOrders_ForCurrentUser()
    {
        // Arrange
        var userId = ObjectId.GenerateNewId().ToString();
        var userOrders = new List<Order>
        {
            new Order { Id = ObjectId.GenerateNewId(), OrderUserInfo = new OrderUserInfo { UserId = ObjectId.Parse(userId) } },
            new Order { Id = ObjectId.GenerateNewId(), OrderUserInfo = new OrderUserInfo { UserId = ObjectId.Parse(userId) } }
        };

        _currentUserServiceMock.SetupGet(u => u.UserId).Returns(userId);

        _orderRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<FilterDefinition<Order>>(), null))
                           .ReturnsAsync(userOrders);

        _mapperMock.Setup(mapper => mapper.Map<List<OrderViewModel>>(userOrders)).Returns(new List<OrderViewModel>
            {
                new OrderViewModel { Id = "1", OrderUserInfo = new OrderUserInfoViewModel { UserId = userId }},
                new OrderViewModel { Id = "2", OrderUserInfo = new OrderUserInfoViewModel { UserId = userId }}
            });

        var orderService = new OrderService(_orderRepositoryMock.Object, null, _currentUserServiceMock.Object, _mapperMock.Object);

        // Act
        var result = await orderService.ListForUserAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userOrders.Count, result.Count); 
    }
}
