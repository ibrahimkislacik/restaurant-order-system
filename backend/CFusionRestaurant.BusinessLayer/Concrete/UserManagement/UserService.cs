
using AutoMapper;
using CFusionRestaurant.BusinessLayer.Abstract.UserManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.UserManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.Settings;
using CFusionRestaurant.ViewModel.UserManagement.Request;
using CFusionRestaurant.ViewModel.UserManagement.Response;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CFusionRestaurant.BusinessLayer.Concrete.UserManagement;

public class UserService : IUserService
{
    private readonly IAppSettings _appSettings;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IAppSettings appSettings,
        IRepository<User> userRepository,
        IMapper mapper)
    {
        _appSettings = appSettings;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserLoginResponseViewModel> LoginAsync(UserLoginRequestViewModel userLoginRequestViewModel)
    {
        var user = await _userRepository.GetAsync(p => p.EMail.Equals(userLoginRequestViewModel.EMail) && p.Password.Equals(userLoginRequestViewModel.Password)).ConfigureAwait(false);
        if (user == null)
        {
            throw new BusinessException("Please check your EMail And Password");
        }
        return GenerateJwtToken(user);
    }

    private UserLoginResponseViewModel GenerateJwtToken(User user)
    {
        var expireDateTime = DateTime.UtcNow.AddMinutes(int.Parse(_appSettings.AccessTokenExpireInMinutes));

        var claims = new List<Claim> {
            new Claim(ClaimTypes.Email, user.EMail),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.IsAdmin ? UserRoles.Admin : UserRoles.User),
        };

        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.SecretKey));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expireDateTime,
            signingCredentials: signinCredentials
            );
        return new UserLoginResponseViewModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            AccessTokenExpireDate = expireDateTime
        };
    }

    public async Task<string> InsertAsync(UserInsertRequestViewModel userInsertRequestViewModel)
    {
        var currentUser = await _userRepository.GetAsync(p => p.EMail.Equals(userInsertRequestViewModel.EMail)).ConfigureAwait(false);
        if (currentUser != null)
        {
            throw new BusinessException($"User with EMail = {userInsertRequestViewModel.EMail} is exist");
        }

        var user = _mapper.Map<User>(userInsertRequestViewModel);
        user.CreatedDateTime = DateTime.Now;
        user.Id = ObjectId.GenerateNewId();
        await _userRepository.InsertAsync(user).ConfigureAwait(false);
        return user.Id.ToString();
    }

}
