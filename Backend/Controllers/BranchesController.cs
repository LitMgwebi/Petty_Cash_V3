using Backend.Services.BranchService;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController(IBranch branch): ControllerBase
    {
        private readonly IBranch _branch = branch;

        #region GET

        [HttpGet, Route("index")]
        public async Task<ActionResult> Index()
        {
            ServerResponse<IEnumerable<Branch>> response = await _branch.GetBranches();

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("details")]
        public async Task<ActionResult> Details(int id)
        {
            ServerResponse<Branch> response = await _branch.GetBranch(id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region POST

        [HttpPost, Route("create")]
        public async Task<ActionResult> Create(Branch branch)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Branch> response = await _branch.CreateBranch(branch);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        #endregion

        #region PUT

        [HttpPut, Route("edit")]
        public async Task<ActionResult> Edit([FromBody]Branch branch)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Branch> response = await _branch.EditBranch(branch);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region DELETE

        // DELETE api/<PurposesController>/5
        [HttpDelete, Route("delete")]
        public async Task<ActionResult> Delete([FromBody]Branch branch)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Branch> response = await _branch.DeleteBranch(branch);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion
    }
}
