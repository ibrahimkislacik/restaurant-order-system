
namespace CFusionRestaurant.BusinessLayer;

public static class OrderNumberGenerator
{
    private static readonly Random Random = new Random();

    public static string GenerateOrderNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

        var randomNumber = Random.Next(1000, 9999);

        return $"{timestamp}-{randomNumber}";

    }
}
