
using CFusionRestaurant.Api.Controllers;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CFusionRestaurant.Api.Tests.CategoryControllerTests;

public class ListCategoryTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;

    public ListCategoryTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
    }

    [Fact]
    public async Task ShouldReturnListOfCategories()
    {
        // Arrange
        var expectedCategories = new List<CategoryViewModel>
        {
            new CategoryViewModel { Id = "1", Name = "Category 1" },
            new CategoryViewModel { Id = "2", Name = "Category 2" }
        };

        _categoryServiceMock.Setup(service => service.ListAsync()).ReturnsAsync(expectedCategories);

        var controller = new CategoryController(_categoryServiceMock.Object);

        // Act
        var result = await controller.List();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualCategories = Assert.IsAssignableFrom<List<CategoryViewModel>>(okResult.Value);
        Assert.Equal(expectedCategories, actualCategories); 
        _categoryServiceMock.Verify(service => service.ListAsync(), Times.Once); 
    }
}
