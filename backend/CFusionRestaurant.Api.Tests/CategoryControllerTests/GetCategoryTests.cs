
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using CFusionRestaurant.Entities.ProductManagement;

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
    public async Task ShouldReturnsCategory()
    {
        // Arrange
        var categoryId = ObjectId.GenerateNewId();
        var expectedCategory = new CategoryViewModel { Id = categoryId.ToString(), Name = "Test Category" };
        _categoryServiceMock.Setup(service => service.GetAsync(categoryId.ToString())).ReturnsAsync(expectedCategory);

        // Act
        var result = await _controller.GetById(categoryId.ToString());

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(expectedCategory);
    }

    [Fact]
    public async Task ShouldReturnsNotFound()
    {
        // Arrange
        string categoryId = ObjectId.GenerateNewId().ToString();
        _categoryServiceMock.Setup(service => service.GetAsync(categoryId)).ReturnsAsync((CategoryViewModel)null);

        // Act
        var result = await _controller.GetById(categoryId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>()
           .Which.Value.Should().Be($"Category with Id = {categoryId} not found");
    }
}
