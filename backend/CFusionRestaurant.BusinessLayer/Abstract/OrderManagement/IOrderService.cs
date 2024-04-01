using CFusionRestaurant.ViewModel.OrderManagement;
using CFusionRestaurant.ViewModel.OrderManagement.Request;
using CFusionRestaurant.ViewModel.OrderManagement.Response;

namespace CFusionRestaurant.BusinessLayer.Abstract.OrderManagement;

public interface IOrderService
{
    Task<OrderInsertResponseViewModel> InsertAsync(OrderInsertRequestViewModel orderInsertRequestViewModel);

    Task<List<OrderViewModel>> ListAsync();

    Task<List<OrderViewModel>> ListForUserAsync();

    Task<OrderViewModel> GetAsync(string id);
}
