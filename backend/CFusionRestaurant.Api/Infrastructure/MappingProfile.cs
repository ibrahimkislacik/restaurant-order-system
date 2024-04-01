using AutoMapper;
using CFusionRestaurant.Entities.OrderManagement;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.Entities.UserManagement;
using CFusionRestaurant.ViewModel.OrderManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using CFusionRestaurant.ViewModel.UserManagement.Request;

namespace CFusionRestaurant.Api.Infrastructure;

/// <summary>
/// AutoMapper profile for mapping between domain models and view models.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryViewModel>(MemberList.Destination).ReverseMap();
        CreateMap<CategoryInsertRequestViewModel, Category>(MemberList.Destination);

        CreateMap<Product, ProductViewModel>(MemberList.Destination).ReverseMap();
        CreateMap<ProductInsertRequestViewModel, Product>(MemberList.Destination);
        CreateMap<ProductUpdateRequestViewModel, Product>(MemberList.Destination);

        CreateMap<Order, OrderViewModel>(MemberList.Destination).ReverseMap();
        CreateMap<OrderProduct, OrderProductViewModel>(MemberList.Destination).ReverseMap();
        CreateMap<OrderUserInfo, OrderUserInfoViewModel>(MemberList.Destination).ReverseMap();

        CreateMap<UserInsertRequestViewModel,User>(MemberList.Destination).ReverseMap();
    }
 }
