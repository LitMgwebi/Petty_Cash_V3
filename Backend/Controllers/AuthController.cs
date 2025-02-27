using Backend.Services.AuthService;
using Backend.ViewModel;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuth auth) : ControllerBase
    {
        private readonly IAuth _auth = auth;

        [HttpPost, Route("register")]
        public async Task<ActionResult> Register([FromBody] UserRegister register)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });

            ServerResponse<JWTToken> response = await _auth.Register(register);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost, Route("login")]
        public async Task<ActionResult> Login([FromBody] UserLogin login)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });

            ServerResponse<JWTToken> response = await _auth.Login(login);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("confirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            ServerResponse<string> response = await _auth.ConfirmEmail(userId, token);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpPut, Route("changePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePassword changePassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model state passed is incorrect." });

            ServerResponse<string> response = await _auth.ChangePassword(changePassword);

            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
