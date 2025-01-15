using Backend.Services.OfficeService;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OfficesController : ControllerBase
    {
        private readonly IOffice _office;
        public OfficesController(IOffice office) => _office = office;

        #region GET

        [HttpGet, Route("index")]
        public async Task<ActionResult<IEnumerable<Office>>> Index()
        {
            ServerResponse<IEnumerable<Office>> response = await _office.GetOffices();

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("details")]
        public async Task<ActionResult<Office>> Details(int id)
        {
            ServerResponse<Office> response = await _office.GetOffice(id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region POST

        [HttpPost, Route("create")]
        public async Task<ActionResult> Create(Office office)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Office> response = await _office.CreateOffice(office);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region PUT

        [HttpPut, Route("edit")]
        public async Task<ActionResult> Edit(Office office)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Office> response = await _office.EditOffice(office);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region DELETE

        [HttpDelete, Route("delete")]
        public async Task<ActionResult> Delete(Office office)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });
            ServerResponse<Office> response = await _office.DeleteOffice(office);

            return response.Success ? Ok(response) : BadRequest(response);
        }
        #endregion
    }
}

