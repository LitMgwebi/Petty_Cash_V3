using Backend.Services.GLAccountService;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Finance_Admin, ICT_Admin")]
    public class GLAccountsController : ControllerBase
    {
        private readonly IGLAccount _glAccount;
        public GLAccountsController(IGLAccount glAccount)
        {
            _glAccount = glAccount;
        }

        #region GET

        [HttpGet, Route("index")]
        [AllowAnonymous]
        public async Task<ActionResult> Index(string command)
        {
            ServerResponse<IEnumerable<Glaccount>> response = await _glAccount.GetGlAccounts(command);

            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }
        [HttpGet, Route("details")]
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            ServerResponse<Glaccount> response = await _glAccount.GetGlAccount(id);

            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        #endregion

        #region POST

        [HttpPost, Route("create")]
        [Authorize(Roles = "ICT_Admin")]
        public async Task<ActionResult> Create(Glaccount glAccount)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Glaccount> response = await _glAccount.CreateGlAccount(glAccount);

            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        #endregion

        #region PUT

        [HttpPut, Route("edit")]
        [Authorize(Roles = "ICT_Admin")]
        public async Task<ActionResult> Edit(Glaccount glAccount)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Glaccount> response = await _glAccount.EditGlAccount(glAccount);

            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        #endregion

        #region DELETE

        [HttpDelete, Route("delete")]
        [Authorize(Roles = "ICT_Admin")]
        public async Task<ActionResult> Delete(Glaccount glAccount)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Glaccount> response = await _glAccount.DeleteGlAccount(glAccount);

            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        #endregion
    }
}
