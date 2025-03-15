using Backend.Services.AccountSetService;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountSetsController : ControllerBase
    {
        private readonly IAccountSet _account;
        public AccountSetsController(IAccountSet account)
        {
            _account = account;
        }

        #region GET

        [HttpGet, Route("index_main")]
        public async Task<ActionResult> IndexMainAccount()
        {
            ServerResponse<IEnumerable<AccountSet>> response = await _account.GetAllAccountSets(AccountSet.MainAccount);

            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet, Route("index_sub")]
        public async Task<ActionResult> IndexSubAccount()
        {
            ServerResponse<IEnumerable<AccountSet>> response = await _account.GetAllAccountSets(AccountSet.SubAccount);

            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet, Route("details_main")]
        public async Task<ActionResult> DetailsMainAccount(int id)
        {
            ServerResponse<AccountSet> response = await _account.GetOneAccountSet(AccountSet.MainAccount, id);
            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }
        
        [HttpGet, Route("details_sub")]
        public async Task<ActionResult> DetailsSubAccount(int id)
        {
            ServerResponse<AccountSet> response = await _account.GetOneAccountSet(AccountSet.SubAccount, id);
            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        #endregion

        #region POST

        [HttpPost, Route("create_main")]
        public async Task<ActionResult<AccountSet>> CreateMainAccount([FromBody] AccountSet account)
        {
            ServerResponse<AccountSet> response = await _account.CreateAccountSet(account, AccountSet.MainAccount);
            if (response.Success == false) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost, Route("create_sub")]
        public async Task<ActionResult<AccountSet>> CreateSubAccount([FromBody] AccountSet account)
        {
            ServerResponse<AccountSet> response = await _account.CreateAccountSet(account, AccountSet.SubAccount);
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

