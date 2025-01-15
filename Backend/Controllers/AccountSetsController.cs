using Backend.Services.AccountSetService;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountSetsController : ControllerBase
    {
        private readonly IAccountSet _account;
        public AccountSetsController(IAccountSet account)
        {
            _account = account;
        }

        #region GET

        [HttpGet, Route("index")]
        public async Task<ActionResult> Index(string command)
        {
            ServerResponse<IEnumerable<AccountSet>> response = await _account.GetAllAccountSets(command);

            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet, Route("details")]
        public async Task<ActionResult> Details(string command, int id)
        {
            ServerResponse<AccountSet> response = await _account.GetOneAccountSet(command, id);
            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        #endregion

        #region POST

        [HttpPost, Route("create")]
        public async Task<ActionResult<AccountSet>> Create([FromBody] AccountSet account, string command)
        {
            ServerResponse<AccountSet> response = await _account.CreateAccountSet(account, command);
            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        #endregion

        #region PUT

        [HttpPut, Route("edit")]
        public async Task<ActionResult> Edit([FromBody] AccountSet account)
        {
            ServerResponse<AccountSet> response = await _account.EditAccountSet(account);
            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        #endregion

        #region DELETE

        [HttpDelete, Route("delete")]
        public async Task<ActionResult> Delete([FromBody] AccountSet account)
        {
            ServerResponse<AccountSet> response = await _account.DeleteAccountSet(account);
            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        #endregion
    }
}

