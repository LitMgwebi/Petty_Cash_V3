using Backend.Services.TransactionService;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Finance_Admin, ICT_Admin")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransaction _transaction;
        public TransactionsController(ITransaction transaction) => _transaction = transaction;

        #region GET

        [HttpGet, Route("index")]
        public async Task<ActionResult> Index(string type)
        {
            ServerResponse<IEnumerable<Transaction>> response = await _transaction.GetTransactions();

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("details")]
        public async Task<ActionResult> Details(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });

            ServerResponse<Transaction> response = await _transaction.GetTransaction(id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region POST

        [HttpPost, Route("create")]
        public async Task<ActionResult> Create(Transaction transaction)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });

            ServerResponse<Transaction> response = await _transaction.CreateTransaction(transaction.Amount, Transaction.Deposit, note: "Reimbursing Vault");

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion
    }
}

