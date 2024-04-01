using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.CategoryControllerTests;

public class DeleteCategoryTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly CategoryController _controller;

    public DeleteCategoryTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _controller = new CategoryController(_categoryServiceMock.Object);
    }

    [Fact]
    public async Task ShouldReturnsNoContent()
    {
        // Arrange
        var categoryId = ObjectId.GenerateNewId().ToString();
        _categoryServiceMock.Setup(service => service.DeleteAsync(categoryId)).Verifiable();

        // Act
        var result = await _controller.Delete(categoryId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _categoryServiceMock.Verify(service => service.DeleteAsync(categoryId), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnsNotFound()
    {
        // Arrange
        var categoryId = ObjectId.GenerateNewId().ToString();
        _categoryServiceMock.Setup(service => service.DeleteAsync(categoryId)).ThrowsAsync(new NotFoundException($"Category with Id = {categoryId} not found"));

        // Act
        Func<Task> act = async () => await _controller.Delete(categoryId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Category with Id = {categoryId} not found");
    }

}
