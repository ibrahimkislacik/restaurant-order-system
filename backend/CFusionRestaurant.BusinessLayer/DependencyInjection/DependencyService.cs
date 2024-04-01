
using CFusionRestaurant.BusinessLayer.Abstract.OrderManagement;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.BusinessLayer.Abstract.UserManagement;
using CFusionRestaurant.BusinessLayer.Concrete.OrderManagement;
using CFusionRestaurant.BusinessLayer.Concrete.ProductManagement;
using CFusionRestaurant.BusinessLayer.Concrete.UserManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.OrderManagement;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.Entities.UserManagement;
using CFusionRestaurant.ViewModel.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CFusionRestaurant.BusinessLayer.DependencyInjection;

/// <summary>
/// Static class responsible for adding dependency services to the service collection.
/// </summary>
public static class DependencyService
{
    public static IServiceCollection AddDependencyServices(this IServiceCollection serviceCollection, IConfiguration configuration) {

        _ = serviceCollection.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)))
            .AddSingleton<IDbSettings>(sp => sp.GetRequiredService<IOptions<DbSettings>>().Value);

        _ = serviceCollection.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)))
            .AddSingleton<IAppSettings>(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);

        _ = serviceCollection.AddSingleton<IRepository<Category>>(provider =>
        {
            var dbSettings = provider.GetService<IDbSettings>();
            return new Repository<Category>(dbSettings!);
        });

        _ = serviceCollection.AddSingleton<IRepository<Product>>(provider =>
        {
            var dbSettings = provider.GetService<IDbSettings>();
            return new Repository<Product>(dbSettings!);
        });

        _ = serviceCollection.AddSingleton<IRepository<Order>>(provider =>
        {
            var dbSettings = provider.GetService<IDbSettings>();
            return new Repository<Order>(dbSettings!);
        });

        _ = serviceCollection.AddSingleton<IRepository<User>>(provider =>
        {
            var dbSettings = provider.GetService<IDbSettings>();
            return new Repository<User>(dbSettings!);
        });

        _ = serviceCollection.AddSingleton<ICategoryService, CategoryService>();
        _ = serviceCollection.AddSingleton<IProductService, ProductService>();
        _ = serviceCollection.AddSingleton<IOrderService, OrderService>();
        _ = serviceCollection.AddSingleton<IUserService, UserService>();

        return serviceCollection;
    }



}
