
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.ProductControllerTests;

public class ListProductTests
{
    private readonly Mock<IProductService> _productServiceMock;

    public ListProductTests()
    {
        _productServiceMock = new Mock<IProductService>();
    }

    [Fact]
    public async Task ShouldReturnsListOfProductsBasedOnDayOfWeek()
    {
        // Arrange
        var dayOfWeek = DayOfWeek.Monday; 
        var expectedProducts = new List<ProductViewModel>
            {
                new ProductViewModel { Id = ObjectId.GenerateNewId().ToString(), Name = "Product 1" },
                new ProductViewModel { Id = ObjectId.GenerateNewId().ToString(), Name = "Product 2" }
            };

        _productServiceMock.Setup(service => service.ListAsync(dayOfWeek)).ReturnsAsync(expectedProducts);

        var controller = new ProductController(_productServiceMock.Object);

        // Act
        var result = await controller.List(dayOfWeek);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(expectedProducts);
    }
}
