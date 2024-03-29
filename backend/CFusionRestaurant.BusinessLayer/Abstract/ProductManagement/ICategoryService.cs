using CFusionRestaurant.ViewModel.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;

namespace CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;

public interface ICategoryService
{
    Task<List<CategoryViewModel>> ListAsync();

    Task<string> InsertAsync(CategoryInsertRequestViewModel categoryInsertViewModel);

    Task<CategoryViewModel> GetAsync(string id);

    Task DeleteAsync(string id);

    Task UpdateAsync(CategoryUpdateRequestViewModel categoryUpdateViewModel);
}
