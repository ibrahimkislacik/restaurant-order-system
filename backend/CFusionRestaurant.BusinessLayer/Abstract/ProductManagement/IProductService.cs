
using CFusionRestaurant.ViewModel.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;

namespace CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;

public interface IProductService
{
    Task<List<ProductViewModel>> ListAsync(DayOfWeek dayOfWeek);

    Task<string> InsertAsync(ProductInsertRequestViewModel productInsertViewModel);

    Task<ProductViewModel> GetAsync(string id);

    Task DeleteAsync(string id);

    Task UpdateAsync(ProductUpdateRequestViewModel productUpdateViewModel);
}
