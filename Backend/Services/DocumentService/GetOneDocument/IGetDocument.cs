namespace Backend.Services.DocumentService.GetOneDocument
{
    public interface IGetDocument
    {
        Task<ServerResponse<Document>> GetOneDocument();
    }
}
