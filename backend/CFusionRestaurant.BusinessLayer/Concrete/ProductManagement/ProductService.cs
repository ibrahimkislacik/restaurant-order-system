using AutoMapper;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using CFusionRestaurant.ViewModel.ProductManagement;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;

    public ProductService(IRepository<Product> productRepository,
        IRepository<Category> categoryRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductViewModel>> ListAsync(DayOfWeek dayOfWeek)
    {
        // Construct filter based on the provided dayOfWeek
        var filter = Builders<Product>.Filter.Empty;
        switch (dayOfWeek)
        {
            case DayOfWeek.Monday:
                filter = Builders<Product>.Filter.Where(p => p.IsActiveOnMonday);
                break;
            case DayOfWeek.Tuesday:
                filter = Builders<Product>.Filter.Where(p => p.IsActiveOnTuesday);
                break;
            case DayOfWeek.Wednesday:
                filter = Builders<Product>.Filter.Where(p => p.IsActiveOnWednesday);
                break;
            case DayOfWeek.Thursday:
                filter = Builders<Product>.Filter.Where(p => p.IsActiveOnThursday);
                break;
            case DayOfWeek.Friday:
                filter = Builders<Product>.Filter.Where(p => p.IsActiveOnFriday);
                break;
            case DayOfWeek.Saturday:
                filter = Builders<Product>.Filter.Where(p => p.IsActiveOnSaturday);
                break;
            case DayOfWeek.Sunday:
                filter = Builders<Product>.Filter.Where(p => p.IsActiveOnSunday);
                break;
        }

        var products = await _productRepository.ListAsync(filter).ConfigureAwait(false);
        return _mapper.Map<List<ProductViewModel>>(products);
    }

    public async Task<string> InsertAsync(ProductInsertRequestViewModel productInsertViewModel)
    {
        if (!productInsertViewModel.IsActiveOnMonday &&
            !productInsertViewModel.IsActiveOnTuesday &&
            !productInsertViewModel.IsActiveOnWednesday &&
            !productInsertViewModel.IsActiveOnThursday &&
            !productInsertViewModel.IsActiveOnFriday &&
            !productInsertViewModel.IsActiveOnSaturday &&
            !productInsertViewModel.IsActiveOnSunday)
        {
            // All properties are false, return businesss exception message
            throw new BusinessException("At least one day must be active");
        }

        var category = await _categoryRepository.GetAsync(productInsertViewModel.CategoryId).ConfigureAwait(false);
        if (category == null)
        {
            //category not found, return business exception
            throw new BusinessException($"Category with Id = {productInsertViewModel.CategoryId} not found");
        }

        var product = _mapper.Map<Product>(productInsertViewModel);
        product.CreatedDateTime = DateTime.Now;
        product.Id = ObjectId.GenerateNewId();
        await _productRepository.InsertAsync(product).ConfigureAwait(false);
        return product.Id.ToString();
    }

    public async Task<ProductViewModel> GetAsync(string id)
    {
        var product = await _productRepository.GetAsync(id).ConfigureAwait(false);
        return _mapper.Map<ProductViewModel>(product);
    }

    public async Task DeleteAsync(string id)
    {
        var product = await _productRepository.GetAsync(id).ConfigureAwait(false);
        if (product == null)
        {
            throw new NotFoundException($"Product with Id = {id} not found");
        }
        await _productRepository.DeleteAsync(id).ConfigureAwait(false);
    }

    public async Task UpdateAsync(ProductUpdateRequestViewModel productUpdateViewModel)
    {
        var product = await _productRepository.GetAsync(productUpdateViewModel.Id).ConfigureAwait(false);
        if (product == null)
        {
            throw new NotFoundException($"Product with Id = {productUpdateViewModel.Id} not found");
        }
        product.Name = productUpdateViewModel.Name;
        product.Description = productUpdateViewModel.Description;
        product.Price = productUpdateViewModel.Price;
        product.IsActiveOnMonday = productUpdateViewModel.IsActiveOnMonday;
        product.IsActiveOnTuesday = productUpdateViewModel.IsActiveOnTuesday;
        product.IsActiveOnWednesday = productUpdateViewModel.IsActiveOnWednesday;
        product.IsActiveOnThursday = productUpdateViewModel.IsActiveOnThursday;
        product.IsActiveOnFriday = productUpdateViewModel.IsActiveOnFriday;
        product.IsActiveOnSaturday = productUpdateViewModel.IsActiveOnSaturday;
        product.IsActiveOnSunday = productUpdateViewModel.IsActiveOnSunday;

        await _productRepository.UpdateAsync(product).ConfigureAwait(false);
    }
}
