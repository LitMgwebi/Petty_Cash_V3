using Backend.ViewModel;

namespace Backend.Services.AuthService
{
    public interface IAuth
    {
        public Task<bool> ValidateUser(UserLogin login, User user);
        public bool ValidateUserRole(string roleNameRequired);
        public Task<ServerResponse<string>> ConfirmEmail(string userId, string token);
        public Task<ServerResponse<JWTToken>> Register(UserRegister userRegister);
        public Task<ServerResponse<JWTToken>> Login(UserLogin login);
        public Task<ServerResponse<string>> ChangePassword(ChangePassword changePassword);
    }
}
