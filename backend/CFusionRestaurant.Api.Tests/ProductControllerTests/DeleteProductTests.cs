
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.Api.Tests.ProductControllerTests;

public class DeleteProductTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly ProductController _controller;

    public DeleteProductTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _controller = new ProductController(_productServiceMock.Object);
    }

    [Fact]
    public async Task ShouldReturnsNoContent()
    {
        // Arrange
        var productId = ObjectId.GenerateNewId().ToString();

        _productServiceMock.Setup(service => service.DeleteAsync(productId)).Verifiable();

        // Act
        var result = await _controller.Delete(productId);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _productServiceMock.Verify(service => service.DeleteAsync(productId), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnsNotFound()
    {
        // Arrange
        var productId = ObjectId.GenerateNewId().ToString();

        _productServiceMock.Setup(service => service.DeleteAsync(productId))
            .ThrowsAsync(new NotFoundException($"Product with Id = {productId} not found"));

        Func<Task> act = async () => await _controller.Delete(productId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Product with Id = {productId} not found");
    }
}
