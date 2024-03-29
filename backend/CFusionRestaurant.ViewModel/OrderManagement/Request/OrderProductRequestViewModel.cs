
using System.ComponentModel.DataAnnotations;

namespace CFusionRestaurant.ViewModel.OrderManagement.Request;

public class OrderProductRequestViewModel
{
    [Required]
    public string ProductId { get; set; }

    [Required]
    [Range(0, 10, ErrorMessage = "Price must be between 0 and 10")]
    public int Quantity { get; set; }

    public string? Note { get; set; }
}
