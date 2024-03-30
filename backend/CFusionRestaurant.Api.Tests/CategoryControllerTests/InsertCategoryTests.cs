
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.CategoryControllerTests;

public class InsertCategoryTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly CategoryController _controller;

    public InsertCategoryTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _controller = new CategoryController(_categoryServiceMock.Object);
    }

    [Fact]
    public async Task ShouldReturnsCreated()
    {
        // Arrange
        var categoryInsertViewModel = new CategoryInsertRequestViewModel { Name = "Test Category" };
        var categoryId = ObjectId.GenerateNewId();
        _categoryServiceMock.Setup(service => service.InsertAsync(categoryInsertViewModel)).ReturnsAsync(categoryId.ToString());

        // Act
        var result = await _controller.Insert(categoryInsertViewModel);

        // Assert
        result.Should().BeOfType<CreatedResult>()
            .Which.Location.Should().Be($"/category/{categoryId}");
        result.Should().BeOfType<CreatedResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    [Fact]
    public async Task ShouldReturnsBadRequest()
    {
        // Arrange
        var categoryInsertViewModel = new CategoryInsertRequestViewModel(); 
        _controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _controller.Insert(categoryInsertViewModel);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

  


}
