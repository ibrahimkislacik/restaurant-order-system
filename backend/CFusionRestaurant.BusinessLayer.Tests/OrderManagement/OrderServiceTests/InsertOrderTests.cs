
using AutoMapper;
using CFusionRestaurant.BusinessLayer.Abstract.UserManagement;
using CFusionRestaurant.BusinessLayer.Concrete.OrderManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.OrderManagement;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.OrderManagement.Request;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using CFusionRestaurant.ViewModel.ExceptionManagement;

namespace CFusionRestaurant.BusinessLayer.Tests.OrderManagement.OrderServiceTests;

public class InsertOrderTests
{
    private readonly Mock<IRepository<Order>> _orderRepositoryMock;
    private readonly Mock<IRepository<Product>> _productRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IMapper> _mapperMock;

    public InsertOrderTests()
    {
        _orderRepositoryMock = new Mock<IRepository<Order>>();
        _productRepositoryMock = new Mock<IRepository<Product>>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldThrowException_WhenNoProductsSelected()
    {
        // Arrange
        var orderInsertRequestViewModel = new OrderInsertRequestViewModel
        {
            OrderProducts = new List<OrderProductRequestViewModel>()
        };

        var orderService = new OrderService(_orderRepositoryMock.Object, _productRepositoryMock.Object,
                                            _currentUserServiceMock.Object, _mapperMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() => orderService.InsertAsync(orderInsertRequestViewModel));

        // Assert
        Assert.Equal("At least one product must be selected", exception.Message);
    }

    [Fact]
    public async Task ShouldThrowException_WhenInvalidProductSelected()
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

        // No active products available in the repository

        _currentUserServiceMock.SetupGet(u => u.UserId).Returns(ObjectId.GenerateNewId().ToString());

        _productRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<FilterDefinition<Product>>(), null))
                              .ReturnsAsync(new List<Product>());

        var orderService = new OrderService(_orderRepositoryMock.Object, _productRepositoryMock.Object,
                                            _currentUserServiceMock.Object, _mapperMock.Object);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() => orderService.InsertAsync(orderInsertRequestViewModel));

        // Assert
        Assert.Equal($"Product with Id = {productId} not found", exception.Message);
    }

    [Fact]
    public async Task ShouldCreateOrder_WhenValidRequestIsProvided()
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

        var products = new List<Product>
        {
            new Product {
                Id = productId,
                IsActiveOnMonday = true,
                IsActiveOnTuesday = true,
                IsActiveOnWednesday = true,
                IsActiveOnThursday = true,
                IsActiveOnFriday = true,
                IsActiveOnSaturday = true,
                IsActiveOnSunday = true,
                Price = 10
            }
        };

        _currentUserServiceMock.SetupGet(u => u.UserId).Returns(ObjectId.GenerateNewId().ToString());

        _productRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<FilterDefinition<Product>>(), null))
                              .ReturnsAsync(products);

        var orderService = new OrderService(_orderRepositoryMock.Object, _productRepositoryMock.Object,
                                            _currentUserServiceMock.Object, _mapperMock.Object);

        // Act
        var result = await orderService.InsertAsync(orderInsertRequestViewModel);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.OrderNo);
    }
}
