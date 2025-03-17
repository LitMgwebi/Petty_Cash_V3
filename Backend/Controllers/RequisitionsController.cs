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
        public async Task<ActionResult> Index()
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<List<Requisition>> response = await _requisition.GetRequisitions(Requisition.getStates.All);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpGet, Route("indexForOne")]
        public async Task<ActionResult> IndexForOne()
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<List<Requisition>> response = await _requisition.GetRequisitions(Requisition.getStates.ForOne);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpGet, Route("indexForRecommendation")]
        public async Task<ActionResult> IndexForRecommendation()
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<List<Requisition>> response = await _requisition.GetRequisitions(Requisition.getStates.Recommendation);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpGet, Route("indexForApproval")]
        public async Task<ActionResult> IndexForApproval()
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<List<Requisition>> response = await _requisition.GetRequisitions(Requisition.getStates.Approval);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpGet, Route("indexForOpened")]
        public async Task<ActionResult> IndexForOpened()
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<List<Requisition>> response = await _requisition.GetRequisitions(Requisition.getStates.Opened);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpGet, Route("indexForIssued")]
        public async Task<ActionResult> IndexForIssued()
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<List<Requisition>> response = await _requisition.GetRequisitions(Requisition.getStates.Issued);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpGet, Route("indexForReturned")]
        public async Task<ActionResult> IndexForReturned()
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<List<Requisition>> response = await _requisition.GetRequisitions(Requisition.getStates.Returned);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpGet, Route("indexForReceiving")]
        public async Task<ActionResult> IndexForReceiving()
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<List<Requisition>> response = await _requisition.GetRequisitions(Requisition.getStates.Receiving);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpGet, Route("indexForTracking")]
        public async Task<ActionResult> IndexForTracking()
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<List<Requisition>> response = await _requisition.GetRequisitions(Requisition.getStates.Tracking);

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

        [HttpPut, Route("editForRecommendation")]
        public async Task<ActionResult> EditForRecommendation([FromBody] Requisition requisition)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Requisition> response = await _requisition.EditRequisition(requisition, Requisition.editStates.Recommendation);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPut, Route("editForApproval")]
        public async Task<ActionResult> EditForApproval([FromBody] Requisition requisition)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Requisition> response = await _requisition.EditRequisition(requisition, Requisition.editStates.Approval);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPut, Route("edit")]
        public async Task<ActionResult> Edit([FromBody] Requisition requisition)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Requisition> response = await _requisition.EditRequisition(requisition, Requisition.editStates.Edit);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPut, Route("editForIssuing")]
        public async Task<ActionResult> EditForIssuing([FromBody] EditRequisition modelForEdit)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Requisition> response = await _requisition.EditRequisition(modelForEdit.Requisition, Requisition.editStates.Issuing, attemptCode: modelForEdit.attemptCode);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPut, Route("editForExpenses")]
        public async Task<ActionResult> EditForExpenses([FromBody] Requisition requisition)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Requisition> response = await _requisition.EditRequisition(requisition, Requisition.editStates.Expenses);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPut, Route("editForReturn")]
        public async Task<ActionResult> EditForReturn([FromBody] Requisition requisition)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Requisition> response = await _requisition.EditRequisition(requisition, Requisition.editStates.Return);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPut, Route("editForClose")]
        public async Task<ActionResult> EditForClose([FromBody] Requisition requisition)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Requisition> response = await _requisition.EditRequisition(requisition, Requisition.editStates.Close);

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

