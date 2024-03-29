
using CFusionRestaurant.ViewModel.UserManagement.Request;
using CFusionRestaurant.ViewModel.UserManagement.Response;

namespace CFusionRestaurant.BusinessLayer.Abstract.UserManagement;

public interface IUserService
{
    public Task<UserLoginResponseViewModel> LoginAsync(UserLoginRequestViewModel userLoginRequestViewModel);

    Task<string> InsertAsync(UserInsertRequestViewModel userInsertRequestViewModel);
}
