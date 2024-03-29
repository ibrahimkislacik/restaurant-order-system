using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.ProductManagement.ProductServiceTests;

public class DeleteProductTests
{
    private readonly Mock<IRepository<Product>> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public DeleteProductTests()
    {
        _productRepositoryMock = new Mock<IRepository<Product>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldDeleteCategory_WhenProductExists()
    {
        // Arrange
        var productId = ObjectId.GenerateNewId();
        var product = new Product { Id = productId, Name = "Product 1" };

        _productRepositoryMock.Setup(repo => repo.GetAsync(productId.ToString())).ReturnsAsync(product);

        var productService = new ProductService(_productRepositoryMock.Object, null, _mapperMock.Object);

        // Act
        await productService.DeleteAsync(productId.ToString());

        // Assert
        _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId.ToString()), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = ObjectId.GenerateNewId();

        _productRepositoryMock.Setup(repo => repo.GetAsync(productId)).ReturnsAsync((Product)null);


        var productService = new ProductService(_productRepositoryMock.Object, null, _mapperMock.Object);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await productService.DeleteAsync(productId.ToString()));
    }
}
