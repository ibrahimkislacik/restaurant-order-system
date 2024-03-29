using CFusionRestaurant.Entities.Common;

namespace CFusionRestaurant.Entities.OrderManagement;

public class Order : BaseMongoEntity
{
    public string OrderNo { get; set; }
    public OrderUserInfo OrderUserInfo { get; set; }

    public List<OrderProduct> OrderProducts { get; set; }

    public decimal Total { get; set; }
}
