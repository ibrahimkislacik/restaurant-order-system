
using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using FluentAssertions;
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


        //Act
        Func<Task> act = async () => await productService.UpdateAsync(productUpdateViewModel);

        //Assert
        await act.Should().ThrowAsync<NotFoundException>();

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
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Name.Should().Be(productUpdateViewModel.Name);
        updatedProduct.Description.Should().Be(productUpdateViewModel.Description);
        updatedProduct.Price.Should().Be(productUpdateViewModel.Price);
        updatedProduct.IsActiveOnMonday.Should().Be(productUpdateViewModel.IsActiveOnMonday);
        updatedProduct.IsActiveOnTuesday.Should().Be(productUpdateViewModel.IsActiveOnTuesday);
        updatedProduct.IsActiveOnWednesday.Should().Be(productUpdateViewModel.IsActiveOnWednesday);
        updatedProduct.IsActiveOnThursday.Should().Be(productUpdateViewModel.IsActiveOnThursday);
        updatedProduct.IsActiveOnFriday.Should().Be(productUpdateViewModel.IsActiveOnFriday);
        updatedProduct.IsActiveOnSaturday.Should().Be(productUpdateViewModel.IsActiveOnSaturday);
        updatedProduct.IsActiveOnSunday.Should().Be(productUpdateViewModel.IsActiveOnSunday);
    }


}
