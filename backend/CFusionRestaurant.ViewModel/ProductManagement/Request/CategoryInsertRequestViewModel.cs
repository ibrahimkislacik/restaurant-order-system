
using System.ComponentModel.DataAnnotations;

namespace CFusionRestaurant.ViewModel.ProductManagement.Request;

public class CategoryInsertRequestViewModel
{
    [Required]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
    public string Name { get; set; }
}
