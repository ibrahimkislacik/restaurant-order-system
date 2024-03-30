
using AutoMapper;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using FluentAssertions;
using MongoDB.Bson;
using Moq;

namespace CFusionRestaurant.BusinessLayer.Tests.ProductManagement.ProductServiceTests;

public class InsertProductTests
{
    private readonly Mock<IRepository<Product>> _productRepositoryMock;
    private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public InsertProductTests()
    {
        _productRepositoryMock = new Mock<IRepository<Product>>();
        _categoryRepositoryMock = new Mock<IRepository<Category>>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldThrowException_WhenAllDaysArePassive()
    {
        // Arrange
        var productInsertViewModel = new ProductInsertRequestViewModel
        {
            IsActiveOnMonday = false,
            IsActiveOnTuesday = false,
            IsActiveOnWednesday = false,
            IsActiveOnThursday = false,
            IsActiveOnFriday = false,
            IsActiveOnSaturday = false,
            IsActiveOnSunday = false,
        };

        var productService = new ProductService(_productRepositoryMock.Object, _categoryRepositoryMock.Object, _mapperMock.Object);

        // Act
        Func<Task> action = async () => await productService.InsertAsync(productInsertViewModel);

        //Assert
        await action.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task ShouldThrowException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var productInsertViewModel = new ProductInsertRequestViewModel
        {
            CategoryId = ObjectId.GenerateNewId().ToString(),
        };

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>())).ReturnsAsync((Category)null);

        var productService = new ProductService(_productRepositoryMock.Object, _categoryRepositoryMock.Object, _mapperMock.Object);

        // Act
        Func<Task> action = async () => await productService.InsertAsync(productInsertViewModel);

        //Assert
        await action.Should().ThrowAsync<BusinessException>();

    }

    [Fact]
    public async Task ShouldInsertProduct_WhenCategoryExistsAndAtLeastOneDayIsActive()
    {
        // Arrange
        var category = new Category { Id = ObjectId.GenerateNewId() };
        var productInsertViewModel = new ProductInsertRequestViewModel
        {
            CategoryId = category.Id.ToString(),
            Name = "Product 1",
            Price = 1,
            IsActiveOnMonday = true,
            IsActiveOnTuesday = true,
            IsActiveOnWednesday = false,
            IsActiveOnThursday = true,
            IsActiveOnFriday = false,
            IsActiveOnSaturday = false,
            IsActiveOnSunday = true
        };

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>())).ReturnsAsync(category); 

        var product = new Product { Id = ObjectId.GenerateNewId() }; 
        _productRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Product>())).Callback<Product>((p) =>
        {
            p.Id = product.Id;
        });
        _mapperMock.Setup(mapper => mapper.Map<Product>(productInsertViewModel)).Returns(product); 

        var productService = new ProductService(_productRepositoryMock.Object, _categoryRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await productService.InsertAsync(productInsertViewModel);

        // Assert
        result.Should().Be(product.Id.ToString());
    }

    

}
