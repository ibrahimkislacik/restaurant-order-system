
namespace CFusionRestaurant.BusinessLayer.Abstract.UserManagement;

public interface ICurrentUserService
{
    public string? UserId { get; }
    public string? EMail { get; }
    public string? Name { get; }
    public string? RoleName { get; }
}
