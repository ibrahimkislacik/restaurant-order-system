using CFusionRestaurant.Entities.Common;
using MongoDB.Bson;

namespace CFusionRestaurant.Entities.ProductManagement;

public class Product : BaseMongoEntity
{
    public ObjectId CategoryId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsActiveOnMonday { get; set; }
    public bool IsActiveOnTuesday { get; set; }
    public bool IsActiveOnWednesday { get; set; }
    public bool IsActiveOnThursday { get; set; }
    public bool IsActiveOnFriday { get; set; }
    public bool IsActiveOnSaturday { get; set; }
    public bool IsActiveOnSunday { get; set; }
}
