namespace Backend.Services.DocumentService.UploadDocument
{
    public interface IUploadDocument
    {
        Task<ServerResponse<Document>> UploadDocument();
    }
}
