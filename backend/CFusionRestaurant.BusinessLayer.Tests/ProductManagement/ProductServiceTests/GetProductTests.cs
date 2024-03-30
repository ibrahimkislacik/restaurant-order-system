
using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using FluentAssertions;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.ProductManagement.ProductServiceTests;

public class GetProductTests
{
    private readonly Mock<IRepository<Product>> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public GetProductTests()
    {
        _productRepositoryMock = new Mock<IRepository<Product>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldGetProductById()
    {
        // Arrange
        var productId = ObjectId.GenerateNewId();
        var expectedProduct = new Product
        {
            Id = productId,
            Name = "Test Product",
            Description = "Test Description",
            Price = 10,
            IsActiveOnMonday = true,
            IsActiveOnTuesday = true,
            IsActiveOnWednesday = false,
            IsActiveOnThursday = true,
            IsActiveOnFriday = false,
            IsActiveOnSaturday = false,
            IsActiveOnSunday = true
        };

        _productRepositoryMock.Setup(repo => repo.GetAsync(productId.ToString())).ReturnsAsync(expectedProduct);

        var productViewModel = new ProductViewModel()
        {
            Id = productId.ToString(),
            Name = "Test Product",
            Description = "Test Description",
            Price = 10,
            IsActiveOnMonday = true,
            IsActiveOnTuesday = true,
            IsActiveOnWednesday = false,
            IsActiveOnThursday = true,
            IsActiveOnFriday = false,
            IsActiveOnSaturday = false,
            IsActiveOnSunday = true
        };
        _mapperMock.Setup(mapper => mapper.Map<ProductViewModel>(expectedProduct)).Returns(productViewModel);

        var productService = new ProductService(_productRepositoryMock.Object, null, _mapperMock.Object);

        // Act
        var result = await productService.GetAsync(productId.ToString());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(productViewModel);
    }
}
