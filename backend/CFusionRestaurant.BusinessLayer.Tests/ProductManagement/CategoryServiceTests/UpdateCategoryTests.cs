using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using FluentAssertions;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.ProductManagement.CategoryServiceTests;

/// <summary>
/// Each test method follows the Arrange-Act-Assert pattern
/// </summary>
public class UpdateCategoryTests
{
    private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public UpdateCategoryTests()
    {
        _categoryRepositoryMock = new Mock<IRepository<Category>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = ObjectId.GenerateNewId();

        var categoryUpdateViewModel = new CategoryUpdateRequestViewModel
        {
            Id = categoryId.ToString(),
            Name = "Updated Category"
        };

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(categoryId.ToString())).ReturnsAsync((Category)null);

        var categoryService = new CategoryService(_categoryRepositoryMock.Object, _mapperMock.Object);

        //Act
        Func<Task> act = async () => await categoryService.UpdateAsync(categoryUpdateViewModel);

        //Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldUpdateCategory()
    {
        // Arrange
        var categoryId = ObjectId.GenerateNewId();

        var categoryUpdateViewModel = new CategoryUpdateRequestViewModel
        {
            Id = categoryId.ToString(),
            Name = "Updated Category"
        };

        var existingCategory = new Category
        {
            Id = categoryId,
            Name = "Old Category",
            CreatedDateTime = DateTime.Now
        };

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(categoryId.ToString())).ReturnsAsync(existingCategory);


        var categoryService = new CategoryService(_categoryRepositoryMock.Object, _mapperMock.Object);

        // Act
        await categoryService.UpdateAsync(categoryUpdateViewModel);

        // Assert
        existingCategory.Name.Should().Be(categoryUpdateViewModel.Name);
    }

    
}
