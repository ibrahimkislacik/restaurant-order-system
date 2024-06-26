﻿using CFusionRestaurant.Entities.Common;

namespace CFusionRestaurant.Entities.UserManagement;

public class User : BaseMongoEntity
{
    public string EMail { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
}
