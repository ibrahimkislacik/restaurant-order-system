using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFusionRestaurant.ViewModel.UserManagement.Request
{
    public class UserInsertRequestViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Surname { get; set; }

        [Required]
        public string EMail { get; set; }

        [Required]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
