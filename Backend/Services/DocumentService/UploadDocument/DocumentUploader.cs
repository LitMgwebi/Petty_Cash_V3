namespace Backend.Services.DocumentService.UploadDocument
{
    public class DocumentUploader
    {
        private IUploadDocument state = null!;

        public void setState(IUploadDocument state) { this.state = state; }

        public async Task<ServerResponse<Document>> request()
        {
            return await state.UploadDocument();
        }
    }
}
