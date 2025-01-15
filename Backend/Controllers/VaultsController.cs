using Backend.Services.VaultService;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Finance_Admin, ICT_Admin")]
    public class VaultsController : ControllerBase
    {
        private readonly IVault _vault;
        public VaultsController(IVault vault) => _vault = vault;

        #region GET

        [HttpGet, Route("index")]
        public async Task<ActionResult> Index()
        {
            ServerResponse<IEnumerable<Vault>> response = await _vault.GetVaults();

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("details")]
        public async Task<ActionResult> Details(int id)
        {
            ServerResponse<Vault> response = await _vault.GetVault(id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion
    }
}
