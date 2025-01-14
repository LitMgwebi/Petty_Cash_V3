using AutoMapper;
using Backend.Mappers.UserMapper;
using Backend.Services.DivisonService;
using Backend.Services.JobTitleService;
using Backend.Services.OfficeService;

namespace Backend.Services.UserService
{
    public class UserService(BackendContext db, UserManager<User> userManager, IMapper mapper, IOffice office, IDivision department, IJobTitle jobTitle) : IUser
    {
        private readonly BackendContext _db = db;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        private readonly IOffice _office = office;
        private readonly IDivision _department = department;
        private readonly IJobTitle _jobTitle = jobTitle;

        public async Task<IEnumerable<UserMapper>> GetAll()
        {
            try
            {
                IEnumerable<User> users = await _db.Users
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

                IEnumerable<UserMapper> userMapped = (users
                    .Select(c => _mapper.Map<UserMapper>(c))
                    .ToList());

                if (userMapped == null)
                    throw new Exception("System could not find any users.");

                return userMapped;
            }
            catch { throw; }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                User user = await _db.Users
                    .AsNoTracking()
                    .Include(d => d.Division)
                    .ThenInclude(a => a.Department)
                    .Include(j => j.JobTitle)
                    .Where(a => a.IsActive == true)
                    .FirstOrDefaultAsync(e => e.Email == email);

                if (user == null) throw new Exception("System was not able to find user");

                return user;
            }
            catch { throw; }
        }

        public async Task<User> GetUserById(string id)
        {
            try
            {
                User user = await _db.Users
                    .Include(d => d.Division)
                    .Include(j => j.JobTitle)
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (user == null) throw new Exception("System was not able to retrieve user");

                return user;
            }
            catch { throw; }
        }

        public async Task<UserMapper> GetMappedUserByEmail(User user)
        {
            try
            {
                IEnumerable<string> roles = await _userManager.GetRolesAsync(user);
                if (roles == null)
                {
                    throw new Exception("You have not been given a role by system administrator.");
                }
                UserMapper userMapper = _mapper.Map<UserMapper>(user);
                userMapper.Role = roles.FirstOrDefault()!;

                return userMapper;
            }
            catch { throw; }
        }

        public async Task<string> Create(User user, string password)
        {
            try
            {
                //user.Office = await _office.GetOne(user.OfficeId);
                //user.Division = await _department.GetOne(user.DivisionId);
                //user.JobTitle = await _jobTitle.GetOne(user.JobTitleId);
                IdentityResult result = await _userManager.CreateAsync(user, password);

                //if (user.JobTitle.Description.Contains("GM"))
                //    result = await _userManager.AddToRoleAsync(user, "GM_Manager");
                //else if (user.JobTitle.Description == "Manager")
                //    result = await _userManager.AddToRoleAsync(user, "Manager");
                //else if (user.JobTitle.Description == "Staff")
                //    result = await _userManager.AddToRoleAsync(user, "Employee");
                if (result.Succeeded)
                {
                    await _db.SaveChangesAsync();
                    return user.Id;
                }
                else
                    throw new DBConcurrencyException($"System could not save user: {new { errors = result.Errors }}");
            }
            catch { throw; }
        }
}
}
