using Backend.Services.DivisonService;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DivisionsController : ControllerBase
    {
        private readonly IDivision _division;
        public DivisionsController(IDivision division) => _division = division;

        #region GET

        [HttpGet, Route("index")]
        public async Task<ActionResult> Index()
        {
            ServerResponse<IEnumerable<Division>> response = await _division.GetDivisions();

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("details")]
        public async Task<ActionResult> Details(int id)
        {
            ServerResponse<Division> response = await _division.GetDivision(id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region POST

        [HttpPost, Route("create")]
        public async Task<ActionResult> Create(Division division)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Division> response = await _division.CreateDivision(division);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region PUT

        [HttpPut, Route("edit")]
        public async Task<ActionResult> Edit(Division division)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Division> response = await _division.EditDivision(division);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region DELETE

        [HttpDelete, Route("delete")]
        public async Task<ActionResult> Delete(Division division)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Division> response = await _division.DeleteDivision(division);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion
    }
}
