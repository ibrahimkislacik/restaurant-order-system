using MongoDB.Bson;
namespace CFusionRestaurant.Entities.OrderManagement;

public class OrderProduct
{
    public ObjectId Id { get; set; }
    public ObjectId ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Note { get; set; }
}
