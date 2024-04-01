
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.ProductControllerTests;

public class UpdateProductTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly ProductController _controller;

    public UpdateProductTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _controller = new ProductController(_productServiceMock.Object);
    }

    [Fact]
    public async Task ShouldReturnsNoContent()
    {
        // Arrange
        var productUpdateViewModel = new ProductUpdateRequestViewModel
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "New Product Name",
            Price = 3,
            IsActiveOnFriday = true
        };

        _productServiceMock.Setup(service => service.UpdateAsync(productUpdateViewModel)).Verifiable();

        // Act
        var result = await _controller.Update(productUpdateViewModel);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _productServiceMock.Verify(service => service.UpdateAsync(productUpdateViewModel), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnsBadRequest()
    {
        // Arrange
        var productUpdateViewModel = new ProductUpdateRequestViewModel();

        _controller.ModelState.AddModelError("Name", "Error message"); 

        // Act
        var result = await _controller.Update(productUpdateViewModel);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task ShouldReturnsNotFound()
    {
        // Arrange
        var productUpdateViewModel = new ProductUpdateRequestViewModel
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "New Name"
        };

        _productServiceMock.Setup(service => service.UpdateAsync(productUpdateViewModel))
            .ThrowsAsync(new NotFoundException($"Product with Id = {productUpdateViewModel.Id} not found"));

        // Act
        Func<Task> act = async () => await _controller.Update(productUpdateViewModel);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Product with Id = {productUpdateViewModel.Id} not found");

    }
}
