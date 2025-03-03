namespace Backend.Services.DepartmentService
{
    public class DepartmentService(BackendContext db): IDepartment
    {
        private BackendContext _db = db;

        public async Task<ServerResponse<IEnumerable<Department>>> GetDepartments()
        {
            try
            {
                IEnumerable<Department> departments = await _db.Departments
                    .Where(x => x.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

                if (departments == null)
                    return new ServerResponse<IEnumerable<Department>>
                    {
                        Success = false,
                        Message = "System could not retrieve the departments."
                    };

                return new ServerResponse<IEnumerable<Department>>
                {
                    Success = true,
                    Data = departments,
                    Message = "Departments retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Department>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        public async Task<ServerResponse<Department>> GetDepartment(int id)
        {
            try
            {
                Department? department = await _db.Departments
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.DepartmentId == id);

                if (department == null)
                    return new ServerResponse<Department>
                    {
                        Success = false,
                        Message = $"System could not retrieve the department with id of #{id}."
                    };

                return new ServerResponse<Department>
                {
                    Success = true,
                    Data = department,
                    Message = $"{department.Name} retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Department>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
