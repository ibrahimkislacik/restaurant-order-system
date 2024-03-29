
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.CategoryControllerTests;

public class GetCategoryTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly CategoryController _controller;

    public GetCategoryTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _controller = new CategoryController(_categoryServiceMock.Object);
    }

    [Fact]
    public async Task WithValidId_ReturnsCategory()
    {
        // Arrange
        var categoryId = ObjectId.GenerateNewId();
        var expectedCategory = new CategoryViewModel { Id = categoryId.ToString(), Name = "Test Category" };
        _categoryServiceMock.Setup(service => service.GetAsync(categoryId.ToString())).ReturnsAsync(expectedCategory);

        // Act
        var result = await _controller.GetById(categoryId.ToString());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualCategory = Assert.IsType<CategoryViewModel>(okResult.Value);
        Assert.Equal(expectedCategory, actualCategory);
    }

    [Fact]
    public async Task WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        string categoryId = ObjectId.GenerateNewId().ToString();
        _categoryServiceMock.Setup(service => service.GetAsync(categoryId)).ReturnsAsync((CategoryViewModel)null);

        // Act
        var result = await _controller.GetById(categoryId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
