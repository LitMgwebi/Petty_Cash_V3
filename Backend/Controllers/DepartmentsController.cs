using Backend.Services.DepartmentService;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartment _department;
        public DepartmentsController(IDepartment department) => _department = department;

        #region GET

        [HttpGet, Route("index")]
        public async Task<ActionResult> Index()
        {
            ServerResponse<IEnumerable<Department>> response = await _department.GetDepartments();

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("details")]
        public async Task<ActionResult<Department>> Details(int id)
        {
            ServerResponse<Department> response = await _department.GetDepartment(id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion
    }
}
