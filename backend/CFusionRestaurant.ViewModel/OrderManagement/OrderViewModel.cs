
namespace CFusionRestaurant.ViewModel.OrderManagement;

public class OrderViewModel
{
    public string Id { get; set; }

    public string OrderNo { get; set; }

    public OrderUserInfoViewModel OrderUserInfo { get; set; }

    public List<OrderProductViewModel> OrderProducts { get; set; }

    public decimal Total { get; set; }
}
