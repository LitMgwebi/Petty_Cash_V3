using Backend.Services.DocumentService;
using Backend.ViewModel;
using Microsoft.AspNetCore.StaticFiles;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentsController(IDocument document, IWebHostEnvironment environment) : ControllerBase
    {
        private IDocument _document = document;
        private readonly IWebHostEnvironment _environment = environment;

        #region GET

        [HttpGet, Route("index")]
        public async Task<ActionResult> Index(string command, int id = 0)
        {
            ServerResponse<IEnumerable<Document>> response = await _document.GetAllDocuments(command, id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("details")]
        public async Task<ActionResult> Details(string command, int id)
        {
            ServerResponse<Document> response = await _document.GetOneDocument(command, id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("download")]
        public async Task<ActionResult> Download(string command, int id)
        {
            ServerResponse<Document> response = await _document.GetOneDocument(command, id);

            if (response.Success == false)
                return BadRequest(response);

            if (response.Data == null)
                return NotFound(response);

            FileExtensionContentTypeProvider provider = new();

            string? contentType;
            string file = Path.Combine(_environment.ContentRootPath, "Resources", "Documents", response.Data.FileName);

            if (!provider.TryGetContentType(file, out contentType))
                contentType = "application/octet-stream";

            byte[] fileBytes;
            if (System.IO.File.Exists(file))
                fileBytes = System.IO.File.ReadAllBytes(file);
            else
                return NotFound(response);

            return File(fileBytes, contentType, response.Data!.FileName, true);
        }

        #endregion

        #region POST

        [HttpPost, Route("create")]
        public async Task<ActionResult> Create([FromBody] UploadFile uploadFile)
        {
            ServerResponse<Document> response = await _document.UploadDocument(uploadFile.command, uploadFile.File, uploadFile.id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        #region DELETE

        [HttpPut, Route("delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteDocument viewModel)
        {
            ServerResponse<Document> response = await _document.DeleteDocument(viewModel.document, viewModel.command);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion
    }
}
