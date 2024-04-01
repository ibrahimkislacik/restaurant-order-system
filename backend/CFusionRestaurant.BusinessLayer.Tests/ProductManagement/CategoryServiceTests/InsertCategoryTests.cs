
using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using FluentAssertions;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.ProductManagement.CategoryServiceTests;

/// <summary>
/// Each test method follows the Arrange-Act-Assert pattern
/// </summary>
public class InsertCategoryTests
{
    private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public InsertCategoryTests()
    {
        _categoryRepositoryMock = new Mock<IRepository<Category>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldInsertCategory_ReturnCategoryId()
    {
        // Arrange
        var categoryInsertViewModel = new CategoryInsertRequestViewModel
        {
            Name = "New Category"
        };

        var expectedCategoryId = ObjectId.GenerateNewId();
        var category = new Category
        {
            Id = expectedCategoryId,
            Name = "New Category",
            CreatedDateTime = DateTime.Now
        };

        _categoryRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Category>())).Callback<Category>((c) =>
        {
            c.Id = expectedCategoryId; 
        });

        _mapperMock.Setup(mapper => mapper.Map<Category>(categoryInsertViewModel)).Returns(category);

        var categoryService = new CategoryService(_categoryRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await categoryService.InsertAsync(categoryInsertViewModel);

        // Assert
        result.Should().Be(expectedCategoryId.ToString());
    }
}
