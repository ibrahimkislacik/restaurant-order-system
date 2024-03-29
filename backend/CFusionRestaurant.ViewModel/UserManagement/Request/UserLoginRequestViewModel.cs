using System.ComponentModel.DataAnnotations;

namespace CFusionRestaurant.ViewModel.UserManagement.Request;

public class UserLoginRequestViewModel
{
    [Required]
    public string EMail { get; set; }

    [Required]
    public string Password { get; set; }
}
