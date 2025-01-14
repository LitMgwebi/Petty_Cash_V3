using Backend.Models;
using Backend.Services.RequisitionService;

namespace Backend.Services.DocumentService.UploadDocument
{
    public class UploadReceiptState(BackendContext db, IRequisition requisition, IFormFile file, string name, int requisitionId, string fileExtension) : IUploadDocument
    {
        private BackendContext _db = db;
        private IFormFile file = file;
        private IRequisition _requisition = requisition;
        private readonly string name = name;
        private readonly int requisitionId = requisitionId;
        private readonly string fileExtension = fileExtension;

        public async Task<ServerResponse<Document>> UploadDocument()
        {
            ServerResponse<Requisition> requisitionResponse = await _requisition.GetRequisition(requisitionId);
            Requisition requisition = requisitionResponse.Data!;
            if (requisition.StateId == Status.Issued || requisition.StateId == Status.Returned)
            {
                Document document = new Receipt();

                document.FileExtension = fileExtension;
                document.DateUploaded = DateTime.Now;
                document.FileName = $"{name}-{document.DateUploaded.ToString("yyyyMMddTHHmmss")}-Petty Cash Receipt-{requisitionId}.{fileExtension}";
                document.RequisitionId = requisitionId;

                string filePath = Path.Combine("Resources", $"{name}-{document.DateUploaded.ToString("yyyyMMddTHHmmss")}-Petty Cash Receipt-{requisitionId}.{fileExtension}");

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                await _db.Documents.AddAsync(document);

                if (await _db.SaveChangesAsync() == 0)
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = "System was unable to add the new receipt."
                    };

                return new ServerResponse<Document>
                {
                    Success = true,
                    Message = "The recipt has been uploaded successfully"
                };
            }
            else
                return new ServerResponse<Document>
                {
                    Success = false,
                    Message = "Cannot upload receipt whilst not in Issued state"
                };
        }

    }
}
