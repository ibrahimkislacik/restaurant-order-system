using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.ProductManagement.CategoryServiceTests;

public class GetCategoryTests
{
    private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public GetCategoryTests()
    {
        _categoryRepositoryMock = new Mock<IRepository<Category>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldGetCategoryById()
    {
        // Arrange
        var categoryId = ObjectId.GenerateNewId();
        var expectedCategory = new Category
        {
            Id = categoryId,
            Name = "Test Category",
        };

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(categoryId.ToString())).ReturnsAsync(expectedCategory);

        var categoryViewModel = new CategoryViewModel()
        {
            Id = categoryId.ToString(),
            Name = "Test Category"
        };
        _mapperMock.Setup(mapper => mapper.Map<CategoryViewModel>(expectedCategory)).Returns(categoryViewModel);

        var categoryService = new CategoryService(_categoryRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await categoryService.GetAsync(categoryId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCategory.Id.ToString(), result.Id);
    }
}
