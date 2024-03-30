using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using FluentAssertions;
using MongoDB.Driver;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.ProductManagement.ProductServiceTests;

public class ListProductTests
{
    private readonly Mock<IRepository<Product>> _productRepositoryMock;
    private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public ListProductTests()
    {
        _productRepositoryMock = new Mock<IRepository<Product>>();
        _categoryRepositoryMock = new Mock<IRepository<Category>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Theory]
    [InlineData(DayOfWeek.Monday)]
    public async Task ShouldReturnProductList_ForGivenDayOfWeek(DayOfWeek dayOfWeek)
    {
        // Arrange
        var products = new List<Product>
            {
                new Product { IsActiveOnMonday = true, IsActiveOnTuesday = false, Name = "Product 1" },
                new Product { IsActiveOnMonday = false, IsActiveOnTuesday = true, Name = "Product 2" },
                new Product { IsActiveOnMonday = true, IsActiveOnTuesday = true, Name = "Product 3" },
            };

        _productRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<FilterDefinition<Product>>(), null))
                      .ReturnsAsync(products);

        var productsViewModel = new List<ProductViewModel>() {
             new ProductViewModel { IsActiveOnMonday = true, IsActiveOnTuesday = false, Name = "Product 1" },
             new ProductViewModel { IsActiveOnMonday = true, IsActiveOnTuesday = true, Name = "Product 3" },
        };
        _mapperMock.Setup(mapper => mapper.Map<List<ProductViewModel>>(products)).Returns(productsViewModel);

        var productService = new ProductService(_productRepositoryMock.Object, _categoryRepositoryMock.Object, _mapperMock.Object);

        // Act
        Func<Task<List<ProductViewModel>>> action = async () => await productService.ListAsync(dayOfWeek);

        // Assert
        await action.Should().NotThrowAsync();
        var result = await action();
        result.Should().NotBeNull();
        result.Count.Should().Be(productsViewModel.Count);
    }
}
