
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.CategoryControllerTests;

public class UpdateCategoryTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly CategoryController _controller;

    public UpdateCategoryTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _controller = new CategoryController(_categoryServiceMock.Object);
    }

    [Fact]
    public async Task ShouldReturnsNoContent()
    {
        // Arrange
        var categoryUpdateViewModel = new CategoryUpdateRequestViewModel
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "New Category Name"
        };

        _categoryServiceMock.Setup(service => service.UpdateAsync(categoryUpdateViewModel));

        // Act
        var result = await _controller.Update(categoryUpdateViewModel);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _categoryServiceMock.Verify(service => service.UpdateAsync(categoryUpdateViewModel), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnsBadRequest()
    {
        // Arrange
        var categoryUpdateViewModel = new CategoryUpdateRequestViewModel();

        _controller.ModelState.AddModelError("Name", "Error message");

        // Act
        var result = await _controller.Update(categoryUpdateViewModel);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task ShouldReturnsNotFound()
    {
        // Arrange
        var categoryUpdateViewModel = new CategoryUpdateRequestViewModel
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "New Category Name"
        };

        _categoryServiceMock.Setup(service => service.UpdateAsync(categoryUpdateViewModel))
            .ThrowsAsync(new NotFoundException($"Category with Id = {categoryUpdateViewModel.Id} not found"));

        // Act
        Func<Task> act = async () => await _controller.Update(categoryUpdateViewModel);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Category with Id = {categoryUpdateViewModel.Id} not found");

    }
}
