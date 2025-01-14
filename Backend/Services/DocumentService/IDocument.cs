namespace Backend.Services.DocumentService
{
    public interface IDocument
    {
        public Task<ServerResponse<IEnumerable<Document>>> GetAllDocuments(string command, int requisitionId);
        public Task<ServerResponse<Document>> GetOneDocument(string command, int documentId);
        public Task<ServerResponse<Document>> UploadDocument(string command, IFormFile file, int id);
        public Task<ServerResponse<Document>> EditDocument(Document document);
        public Task<ServerResponse<Document>> DeleteDocument(Document document, string typeOfDocument);
    }
}
