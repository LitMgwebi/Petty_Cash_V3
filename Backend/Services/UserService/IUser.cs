using Backend.Mappers.UserMapper;

namespace Backend.Services.UserService
{
    public interface IUser
    {
        public Task<IEnumerable<UserMapper>> GetAll();
        public Task<User> GetUserByEmail(string email);
        public Task<User> GetUserById(string id);
        public Task<UserMapper> GetMappedUserByEmail(User user);
        public Task<string> Create(User user, string password);
    }
}
