namespace CFusionRestaurant.ViewModel.Settings;

public interface IDbSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}
