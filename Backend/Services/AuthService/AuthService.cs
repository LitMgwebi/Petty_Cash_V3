using Backend.Services.UserService;
using Backend.ViewModel;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services.AuthService
{
    public class AuthService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IUser user, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IAuth
    {
        #region Dependecy Injection

        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IUser _user = user;
        private readonly IConfiguration _configuration = configuration;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        #endregion

        #region Claim types creation and access

        private ClaimsPrincipal GetUser() => _httpContextAccessor.HttpContext!.User;
        private string GetUserId() => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        private string GetUserRole() => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role)!;
        private string GetUserEmail() => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email)!;

        private async Task<JWTToken> CreateToken(User user)
        {
            try
            {
                JWTToken jwtToken = new JWTToken();
                string issuer = _configuration["Jwt:Issuer"]!;
                string audience = _configuration["Jwt:Audience"]!;
                var key = Environment.GetEnvironmentVariable("JWTKEY");
                var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
                SigningCredentials signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature);

                IList<string> roles = await _userManager.GetRolesAsync(user);
                List<Claim> claims = new List<Claim>();
                if (roles.Count() > 0)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.FirstOrDefault()!));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id!.ToString()));
                    claims.Add(new Claim(ClaimTypes.Email, user.Email!));
                    claims.Add(new Claim(ClaimTypes.Name, user.FullName));
                }
                else
                    throw new InvalidOperationException("You have not been authorized for access by the system's administration. Please contact system administration to gain access.");

                JwtSecurityToken tokenOptions = new JwtSecurityToken
                (
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(20),
                    issuer: issuer,
                    audience: audience,
                    signingCredentials: signingCredentials
                );

                jwtToken.Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return jwtToken;
            }
            catch { throw; }
        }

        #endregion

        #region Validate User

        public async Task<bool> ValidateUser(UserLogin login, User user)
        {
            try
            {
                return (await _userManager.CheckPasswordAsync(user, login.Password));
            }
            catch { throw; }
        }

        public bool ValidateUserRole(string roleNameRequired)
        {
            var user = GetUser();
            return user.IsInRole(roleNameRequired);
        }

        public async Task<ServerResponse<string>> ConfirmEmail(string userId, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                    return new ServerResponse<string>
                    {
                        Success = false,
                        Message = "User profile was not found in the system."
                    };

                var decodedToken = WebEncoders.Base64UrlDecode(token);
                var normalToken = Encoding.UTF8.GetString(decodedToken);

                var result = await _userManager.ConfirmEmailAsync(user, normalToken);

                return result.Succeeded ?
                    new ServerResponse<string>
                    {
                        Success = true,
                        Message = "Email successfully confirmed."
                    } :
                    new ServerResponse<string>
                    {
                        Success = false,
                        Message = "Email was not confirmed.",
                        Data = result.Errors.Select(e => e.Description).ToString()
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        #endregion

        #region Auth functions

        public async Task<ServerResponse<JWTToken>> Register(UserRegister userRegister)
        {
            try
            {
                if (userRegister.Password != userRegister.ConfirmPassword)
                {
                    return new ServerResponse<JWTToken>
                    {
                        Success = false,
                        Message = "Password entered and confirm password do not match."
                    };
                }

                User user = new User
                {
                    Email = userRegister.Email,
                    UserName = userRegister.Email,
                    Firstname = userRegister.FirstName,
                    Lastname = userRegister.LastName,
                    //PhoneNumber = userRegister.PhoneNumber,
                    //DivisionId = userRegister.DivisionId,
                    //JobTitleId = userRegister.JobTitleId,
                    //OfficeId = userRegister.OfficeId,
                    IsActive = true,
                };

                var result = await _userManager.CreateAsync(user, userRegister.Password);

                if (!result.Succeeded)
                    return new ServerResponse<JWTToken>
                    {
                        Success = false,
                        Message = "User registration failed.",
                        Data = (JWTToken)result.Errors.Select(e => e.Description)
                    };

                await ProcessRole(user);

                JWTToken token = await CreateToken(user);
                return new ServerResponse<JWTToken>
                {
                    Success = true,
                    Message = "User registered successfully.",
                    Data = token
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<JWTToken>
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<ServerResponse<JWTToken>> Login(UserLogin login)
        {
            User? user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
                return new ServerResponse<JWTToken>
                {
                    Success = false,
                    Message = "System could not find your user details."
                };

            if (!user.IsActive)
                return new ServerResponse<JWTToken>
                {
                    Success = false,
                    Message = "Access is denied."
                };

            if (!await ValidateUser(login, user))
                return new ServerResponse<JWTToken>
                {
                    Success = false,
                    Message = "Email or password is incorrect. Please retry."
                };

            JWTToken token = await CreateToken(user);

            return new ServerResponse<JWTToken>
            {
                Success = true,
                Message = "You have logged in successfully.",
                Data = token
            };
        }

        public async Task<ServerResponse<string>> ChangePassword(ChangePassword changePassword)
        {
            string loggedInEmail = GetUserEmail();
            User? user = await _userManager.FindByEmailAsync(loggedInEmail);

            if (user == null)
                return new ServerResponse<string>
                {
                    Success = false,
                    Message = "System could not find your user information."
                };

            var result = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);

            return result.Succeeded ?
                new ServerResponse<string>
                {
                    Success = true,
                    Message = "You have successfully changed your password."
                } :
                new ServerResponse<string>
                {
                    Success = false,
                    Message = "System was unable to change your password."
                };
        }

        #endregion

        #region Helper functions

        public async Task ProcessRole(User user)
        {
            IdentityRole? role = new();
            IdentityResult? result = new();

            if (user.JobTitleId == JobTitle.Manager)
            {
                role = await _roleManager.FindByNameAsync("Manager");

                if (role == null)
                    throw new Exception("Manager role was not found.");

                result = await _userManager.AddToRoleAsync(user, role.Name!);
                await AddToManagerClaim(user);
            }
            else if (user.JobTitleId == JobTitle.GeneralManager)
            {
                role = await _roleManager.FindByNameAsync("GM_Manager");

                if (role == null)
                    throw new Exception("GM_Manager role was not found.");

                result = await _userManager.AddToRoleAsync(user, role.Name!);
            }
            else if (user.JobTitleId == JobTitle.CEO)
            {
                role = await _roleManager.FindByNameAsync("CEO");

                if (role == null)
                    throw new Exception("CEO role was not found.");

                result = await _userManager.AddToRoleAsync(user, role.Name!);
            }
            else if (user.JobTitleId == JobTitle.GeneralManager)
            {
                role = await _roleManager.FindByNameAsync("ChairPerson");

                if (role == null)
                    throw new Exception("ChairPerson role was not found.");

                result = await _userManager.AddToRoleAsync(user, role.Name!);
            }

            if (!result.Succeeded)
                throw new Exception($"System was unable to add {user.FullName} to {role.Name}.");
        }

        public async Task AddToManagerClaim(User user)
        {
            string name = "Manager";
            string value = "ManagerClaim";

            var userClaims = await _userManager.GetClaimsAsync(user);

            if (userClaims == null || !userClaims.Any(c => c.Type == name && c.Value == value))
            {
                var claim = new Claim(name, value);
                var result = await _userManager.AddClaimAsync(user, claim);

                if (!result.Succeeded)
                    throw new Exception($"System failed to add manager claim for: {user.FullName}.");
            }
        }

        #endregion
    }
}