using System.ComponentModel.DataAnnotations;

namespace CFusionRestaurant.ViewModel.ProductManagement.Request;

public class CategoryUpdateRequestViewModel
{
    [Required]
    public string Id { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
    public string Name { get; set; }
    
}
