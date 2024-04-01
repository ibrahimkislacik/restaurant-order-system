
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.ProductControllerTests;

public class GetProductTests
{
    private readonly Mock<IProductService> _productServiceMock;

    public GetProductTests()
    {
        _productServiceMock = new Mock<IProductService>();
    }

    [Fact]
    public async Task ShouldReturnsOk()
    {
        // Arrange
        var productId = ObjectId.GenerateNewId().ToString();
        var expectedProduct = new ProductViewModel
        {
            Name = "name",
            Price = 5,
            IsActiveOnFriday = true,
        };

        _productServiceMock.Setup(service => service.GetAsync(productId)).ReturnsAsync(expectedProduct);

        var controller = new ProductController(_productServiceMock.Object);

        // Act
        var result = await controller.GetById(productId);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task ShouldReturnsNotFound()
    {
        // Arrange
        var productId = ObjectId.GenerateNewId().ToString();

        var productServiceMock = new Mock<IProductService>();
        productServiceMock.Setup(service => service.GetAsync(productId)).ReturnsAsync((ProductViewModel)null);

        var controller = new ProductController(productServiceMock.Object);

        // Act
        var result = await controller.GetById(productId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>()
            .Which.Value.Should().Be($"Product with Id = {productId} not found");
    }
}
