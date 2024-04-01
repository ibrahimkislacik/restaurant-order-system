
namespace CFusionRestaurant.ViewModel.Settings;

public interface IAppSettings
{
    public string SecretKey { get; set; }

    public string AccessTokenExpireInMinutes { get; set; }
}
