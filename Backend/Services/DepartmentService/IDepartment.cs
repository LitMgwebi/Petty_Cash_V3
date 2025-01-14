namespace Backend.Services.DepartmentService
{
    public interface IDepartment
    {
        Task<ServerResponse<IEnumerable<Department>>> GetDepartments();
        Task<ServerResponse<Department>> GetDepartment(int id);
    }
}
