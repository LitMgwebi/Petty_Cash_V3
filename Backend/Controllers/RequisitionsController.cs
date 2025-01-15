using Backend.Services.RequisitionService;
using Backend.ViewModel;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequisitionsController : ControllerBase
    {
        private IRequisition _requisition;
        public RequisitionsController(IRequisition requisition)
        {
            _requisition = requisition;
        }

        #region GET

        [HttpGet, Route("index")]
        public async Task<ActionResult> Index([FromQuery] string command, int statusId, string issuedState = "")
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<List<Requisition>> response = await _requisition.GetRequisitions(command);

            return response.Success ? Ok(response) : BadRequest(response);
        }


        [HttpGet, Route("details")]
        public async Task<ActionResult> Details(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Requisition> response = await _requisition.GetRequisition(id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region POST

        [HttpPost, Route("create")]
        public async Task<ActionResult> Create([FromBody] Requisition requisition)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Requisition> response = await _requisition.CreateRequisition(requisition);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region PUT

        [HttpPut, Route("edit")]
        public async Task<ActionResult> Edit([FromBody] EditRequisition modelForEdit)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Requisition> response = await _requisition.EditRequisition(modelForEdit.Requisition, modelForEdit.command, attemptCode: modelForEdit.attemptCode);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        #endregion

        #region DELETE

        [HttpDelete, Route("delete")]
        public async Task<ActionResult> Delete([FromBody] Requisition requisition)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Requisition> response = await _requisition.DeleteRequisition(requisition);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion
    }
}

