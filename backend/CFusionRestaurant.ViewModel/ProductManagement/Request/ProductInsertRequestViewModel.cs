
using System.ComponentModel.DataAnnotations;

namespace CFusionRestaurant.ViewModel.ProductManagement.Request;

public class ProductInsertRequestViewModel
{
    [Required]
    public string CategoryId { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
    public string Name { get; set; }

    [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters")]
    public string? Description { get; set; }

    [Required]
    [Range(0, 1000, ErrorMessage = "Price must be between 0 and 1000")]
    public decimal Price { get; set; }

    [Required]
    public bool IsActiveOnMonday { get; set; }

    [Required]
    public bool IsActiveOnTuesday { get; set; }

    [Required]
    public bool IsActiveOnWednesday { get; set; }

    [Required]
    public bool IsActiveOnThursday { get; set; }

    [Required]
    public bool IsActiveOnFriday { get; set; }

    [Required]
    public bool IsActiveOnSaturday { get; set; }

    [Required]
    public bool IsActiveOnSunday { get; set; }
}
