
using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.ProductManagement.ProductServiceTests;

public class UpdateProductTests
{
    private readonly Mock<IRepository<Product>> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public UpdateProductTests()
    {
        _productRepositoryMock = new Mock<IRepository<Product>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldThrowNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = ObjectId.GenerateNewId();

        var productUpdateViewModel = new ProductUpdateRequestViewModel
        {
            Id = productId.ToString()
        };

        _productRepositoryMock.Setup(repo => repo.GetAsync(productId.ToString())).ReturnsAsync((Product)null);

        var productService = new ProductService(_productRepositoryMock.Object, null, _mapperMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => productService.UpdateAsync(productUpdateViewModel));
    }

    [Fact]
    public async Task ShouldUpdateProduct()
    {
        // Arrange
        var productId = ObjectId.GenerateNewId();

        var productUpdateViewModel = new ProductUpdateRequestViewModel
        {
            Id = productId.ToString(),
            Name = "New Product Name",
            Description = "New Product Description",
            Price = 15,
            IsActiveOnMonday = true,
            IsActiveOnTuesday = true,
            IsActiveOnWednesday = false,
            IsActiveOnThursday = true,
            IsActiveOnFriday = false,
            IsActiveOnSaturday = false,
            IsActiveOnSunday = true
        };

        var existingProduct = new Product
        {
            Id = productId,
            Name = "Old Product Name",
            Description = "Old Product Description",
            Price = 1,
            IsActiveOnMonday = false,
            IsActiveOnTuesday = false,
            IsActiveOnWednesday = false,
            IsActiveOnThursday = false,
            IsActiveOnFriday = false,
            IsActiveOnSaturday = false,
            IsActiveOnSunday = true
        };

        _productRepositoryMock.Setup(repo => repo.GetAsync(productId.ToString())).ReturnsAsync(existingProduct);

        var productService = new ProductService(_productRepositoryMock.Object, null, _mapperMock.Object);

        // Act
        await productService.UpdateAsync(productUpdateViewModel);

        // Assert
        var updatedProduct = await _productRepositoryMock.Object.GetAsync(productId.ToString());
        Assert.NotNull(updatedProduct);
        Assert.Equal(productUpdateViewModel.Name, updatedProduct.Name);
        Assert.Equal(productUpdateViewModel.Description, updatedProduct.Description);
        Assert.Equal(productUpdateViewModel.Price, updatedProduct.Price);
        Assert.Equal(productUpdateViewModel.IsActiveOnMonday, updatedProduct.IsActiveOnMonday);
        Assert.Equal(productUpdateViewModel.IsActiveOnTuesday, updatedProduct.IsActiveOnTuesday);
        Assert.Equal(productUpdateViewModel.IsActiveOnWednesday, updatedProduct.IsActiveOnWednesday);
        Assert.Equal(productUpdateViewModel.IsActiveOnThursday, updatedProduct.IsActiveOnThursday);
        Assert.Equal(productUpdateViewModel.IsActiveOnFriday, updatedProduct.IsActiveOnFriday);
        Assert.Equal(productUpdateViewModel.IsActiveOnSaturday, updatedProduct.IsActiveOnSaturday);
        Assert.Equal(productUpdateViewModel.IsActiveOnSunday, updatedProduct.IsActiveOnSunday);
    }


}
