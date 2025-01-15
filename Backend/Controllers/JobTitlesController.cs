using Backend.Services.JobTitleService;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitlesController(IJobTitle jobTitle) : ControllerBase
    {
        private readonly IJobTitle _jobTitle = jobTitle;

        #region GET

        [HttpGet, Route("index")]
        public async Task<ActionResult> Index()
        {
            ServerResponse<IEnumerable<JobTitle>> response = await _jobTitle.GetJobTitles();

            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet, Route("details")]
        public async Task<ActionResult> Details(int id)
        {
            ServerResponse<JobTitle> response = await _jobTitle.GetJobTitle(id);

            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        #endregion
    }
}
