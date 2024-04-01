
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.ProductControllerTests;

public class InsertProductTests
{
    private readonly Mock<IProductService> _productServiceMock;

    public InsertProductTests()
    {
        _productServiceMock = new Mock<IProductService>();
    }

    [Fact]
    public async Task ShouldReturnsCreated()
    {
        // Arrange
        var productInsertViewModel = new ProductInsertRequestViewModel
        {
            Name = "name",
            Price = 10,
            IsActiveOnFriday = true,
        };
        var expectedResult = ObjectId.GenerateNewId().ToString();

        _productServiceMock.Setup(service => service.InsertAsync(productInsertViewModel)).ReturnsAsync(expectedResult);

        var controller = new ProductController(_productServiceMock.Object);

        // Act
        var result = await controller.Insert(productInsertViewModel);

        // Assert
        result.Should().BeOfType<CreatedResult>()
            .Which.Location.Should().Be($"/product/{expectedResult}");
    }

    [Fact]
    public async Task ShouldReturnsBadRequest()
    {
        // Arrange
        var invalidProductInsertViewModel = new ProductInsertRequestViewModel
        {
            Price = 10,
            IsActiveOnFriday = true,
        };

        var controller = new ProductController(Mock.Of<IProductService>());

        controller.ModelState.AddModelError("Name", "Error Message");

        // Act
        var result = await controller.Insert(invalidProductInsertViewModel);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
