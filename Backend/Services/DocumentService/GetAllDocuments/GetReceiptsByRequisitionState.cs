namespace Backend.Services.DocumentService.GetAllDocuments
{
    public class GetReceiptsByRequisitionState(BackendContext db, int requisitionId) : IIndexDocuments
    {
        private BackendContext db = db;
        private readonly int requisitionId = requisitionId;

        public async Task<ServerResponse<IEnumerable<Document>>> GetDocuments()
        {
            IEnumerable<Document> documents = await db.Documents
                .OfType<Receipt>()
                .Where(x => x.IsActive == true)
                .Where(r => r.RequisitionId == requisitionId)
                .AsNoTracking()
                .ToListAsync();

            if (documents == null)
                return new ServerResponse<IEnumerable<Document>>
                {
                    Success = false,
                    Message = "System could not find the receipts for this requisition."
                };

            return new ServerResponse<IEnumerable<Document>>
            {
                Success = true,
                Data = documents,
                Message = $"Receipts retrieved successfully for requisition #{requisitionId}."
            };
        }

    }
}
