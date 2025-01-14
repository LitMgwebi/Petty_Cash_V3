using AutoMapper;

namespace Backend.Mappers.UserMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserMapper>();
        }
    }
}
