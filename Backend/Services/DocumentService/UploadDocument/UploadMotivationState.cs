using Backend.Models;
using Backend.Services.RequisitionService;

namespace Backend.Services.DocumentService.UploadDocument
{
    public class UploadMotivationState(BackendContext db, IRequisition requisition, IFormFile file, string name, int requisitionId, string fileExtension) : IUploadDocument
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

            if (requisition.ManagerId == null)
            {
                Document document = new Motivation();

                document.FileExtension = fileExtension;
                document.DateUploaded = DateTime.Now;
                document.FileName = $"{name}-{document.DateUploaded.ToString("yyyyMMddTHHmmss")}-Petty Cash Motivation-{requisitionId}.{fileExtension}";
                document.RequisitionId = requisitionId;

                string filePath = Path.Combine("Resources", $"{name}-{document.DateUploaded.ToString("yyyyMMddTHHmmss")}-Petty Cash Motivation-{requisitionId}.{fileExtension}");

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                await _db.Documents.AddAsync(document);

                if (await _db.SaveChangesAsync() == 0)
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = "System was unable to add the new motivation."
                    };

                return new ServerResponse<Document>
                {
                    Success = true,
                    Message = "The motivation has been uploaded successfully"
                };
            }
            else
                return new ServerResponse<Document>
                {
                    Success = false,
                    Message = "A new motivational document cannot be uploaded after line manager has processed requisition."
                };
        }
    }
}
