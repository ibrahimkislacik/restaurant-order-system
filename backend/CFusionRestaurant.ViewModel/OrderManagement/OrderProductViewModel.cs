namespace CFusionRestaurant.ViewModel.OrderManagement;

public class OrderProductViewModel
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Note { get; set; }
}
