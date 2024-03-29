
using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
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
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Name, result.Name);
        Assert.Equal(expectedProduct.Description, result.Description);
        Assert.Equal(expectedProduct.Price, result.Price);
        Assert.Equal(expectedProduct.IsActiveOnMonday, result.IsActiveOnMonday);
        Assert.Equal(expectedProduct.IsActiveOnTuesday, result.IsActiveOnTuesday);
        Assert.Equal(expectedProduct.IsActiveOnWednesday, result.IsActiveOnWednesday);
        Assert.Equal(expectedProduct.IsActiveOnThursday, result.IsActiveOnThursday);
        Assert.Equal(expectedProduct.IsActiveOnFriday, result.IsActiveOnFriday);
        Assert.Equal(expectedProduct.IsActiveOnSaturday, result.IsActiveOnSaturday);
        Assert.Equal(expectedProduct.IsActiveOnSunday, result.IsActiveOnSunday);
    }
}
