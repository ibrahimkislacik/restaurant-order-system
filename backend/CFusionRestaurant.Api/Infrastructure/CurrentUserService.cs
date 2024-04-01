using CFusionRestaurant.BusinessLayer.Abstract.UserManagement;
using System.Security.Claims;

namespace CFusionRestaurant.Api.Infrastructure;

/// <summary>
/// Service for retrieving current user information from the HTTP context.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the ID of the current user.
    /// </summary>
    public string? UserId
    {
        get {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

    public string? EMail
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        }
    }

    public string? Name
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        }
    }

    /// <summary>
    /// Gets the role name of the current user.
    /// </summary>
    public string? RoleName
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
        }
    }
}
