namespace Backend.Services.DocumentService.GetAllDocuments
{
    public interface IIndexDocuments
    {
        Task<ServerResponse<IEnumerable<Document>>> GetDocuments();
    }
}
