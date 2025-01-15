using Backend.Services.StatusService;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusesController : ControllerBase
    {
        private readonly IStatus _status;
        public StatusesController(IStatus status) { _status = status; }

        #region GET

        [HttpGet, Route("index")]
        public async Task<ActionResult> Index()
        {
            ServerResponse<IEnumerable<Status>> response = await _status.GetStatuses();

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("get_approved")]
        public async Task<ActionResult> IndexApproved()
        {
            ServerResponse<IEnumerable<Status>> response = await _status.GetApprovedStatuses();

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("get_recommended")]
        public async Task<ActionResult> IndexRecommended()
        {
            ServerResponse<IEnumerable<Status>> response = await _status.GetRecommendedStatuses();

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("get_requisition_states")]
        public async Task<ActionResult> IndexRequisitionStates()
        {
            ServerResponse<IEnumerable<Status>> response = await _status.GetRequisitionStatesStatuses();

            return response.Success ? Ok(response) : BadRequest(response);
        }


        #endregion
    }
}

