using AutoMapper;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using MongoDB.Bson;
using Moq;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using FluentAssertions;

namespace CFusionRestaurant.BusinessLayer.Tests.ProductManagement.CategoryServiceTests;

/// <summary>
/// Each test method follows the Arrange-Act-Assert pattern
/// </summary>
public class ListCategoryTests
{
    private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public ListCategoryTests()
    {
        _categoryRepositoryMock = new Mock<IRepository<Category>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldReturnListOfCategoryViewModel()
    {
        // Arrange step - for setup the testing objects and prepare the prerequisites for test
        var categories = new List<Category>
            {
                new Category { Id = ObjectId.GenerateNewId(), Name = "Category 1" },
                new Category { Id = ObjectId.GenerateNewId(), Name = "Category 2" }
            };

        _categoryRepositoryMock.Setup(repo => repo.ListAsync()).ReturnsAsync(categories);

        var expectedCategoryViewModels = categories.Select(category =>
        new CategoryViewModel { Id = category.Id.ToString(), Name = category.Name }
        ).ToList();

        _mapperMock.Setup(mapper => mapper.Map<List<CategoryViewModel>>(categories)).Returns(expectedCategoryViewModels);

        var categoryService = new CategoryService(_categoryRepositoryMock.Object, _mapperMock.Object);

        // Act step - perform the actual work of the test
        var result = await categoryService.ListAsync();

        // Assert step - verify the result
        result.Should().NotBeNull().And.BeEquivalentTo(expectedCategoryViewModels);
    }
}
