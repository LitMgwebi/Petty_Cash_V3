namespace Backend.Services.DocumentService.GetAllDocuments
{
    public class GetMotivationsByRequisitionState(BackendContext db, int requisitionId) : IIndexDocuments
    {
        private BackendContext db = db;
        private readonly int requisitionId = requisitionId;

        public async Task<ServerResponse<IEnumerable<Document>>> GetDocuments()
        {
            IEnumerable<Document> documents = await db.Documents
                .OfType<Motivation>()
                .Where(x => x.IsActive == true)
                .Where(r => r.RequisitionId == requisitionId)
                .AsNoTracking()
                .ToListAsync();

            if (documents == null)
                return new ServerResponse<IEnumerable<Document>>
                {
                    Success = false,
                    Message = "System could not find the motivations for this requisition."
                };

            return new ServerResponse<IEnumerable<Document>>
            {
                Success = true,
                Data = documents,
                Message = $"Motivations retrieved successfully for requisition #{requisitionId}."
            };
        }

    }
}
