namespace Backend.Services.DocumentService.GetOneDocument
{
    public class GetOneReceiptState(BackendContext db, int documentId) : IGetDocument
    {
        private BackendContext db = db;
        private readonly int documentId = documentId;

        public async Task<ServerResponse<Document>> GetOneDocument()
        {
            Document document = await db.Documents
                .OfType<Receipt>()
                .Where(a => a.IsActive == true)
                .Include(d => d.Requisition)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.DocumentId == documentId);

            if (document == null)
                return new ServerResponse<Document>
                {
                    Success = false,
                    Message = "System could not retrieve the receipt."
                };

            return new ServerResponse<Document>
            {
                Success = true,
                Data = document,
                Message = $"Receipt retrieved successfully."
            };
        }
    }
}
