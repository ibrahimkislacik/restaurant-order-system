using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using FluentAssertions;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.ProductManagement.CategoryServiceTests;

/// <summary>
/// Each test method follows the Arrange-Act-Assert pattern
/// </summary>
public class DeleteCategoryTests
{
    private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public DeleteCategoryTests()
    {
        _categoryRepositoryMock = new Mock<IRepository<Category>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldDeleteCategory_WhenCategoryExists()
    {
        // Arrange
        var categoryId = ObjectId.GenerateNewId();
        var category = new Category { Id = categoryId, Name = "Category 1" };

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(categoryId.ToString())).ReturnsAsync(category);

        var categoryService = new CategoryService(_categoryRepositoryMock.Object, _mapperMock.Object);

        // Act
        Func<Task> deleteAction = async () => await categoryService.DeleteAsync(categoryId.ToString());

        // Assert
        await deleteAction.Should().NotThrowAsync<Exception>();
        _categoryRepositoryMock.Verify(repo => repo.DeleteAsync(categoryId.ToString()), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = ObjectId.GenerateNewId();

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(categoryId)).ReturnsAsync((Category)null);


        var categoryService = new CategoryService(_categoryRepositoryMock.Object, _mapperMock.Object);

        // Act
        Func<Task> deleteAction = async () => await categoryService.DeleteAsync(categoryId.ToString());

        //Assert
        await deleteAction.Should().ThrowAsync<NotFoundException>();
    }
}
