
namespace CFusionRestaurant.ViewModel.Settings;

public class AppSettings : IAppSettings
{
    public string SecretKey { get; set; }

    public string AccessTokenExpireInMinutes { get; set; }
}
