using MongoDB.Bson;

namespace CFusionRestaurant.Entities.OrderManagement;

public class OrderUserInfo
{
    public ObjectId Id { get; set; }
    public ObjectId UserId { get; set; }
    public string? Name { get; set; }
    
    public string? Surname { get; set; }
    public string? EMail { get; set; }
}
