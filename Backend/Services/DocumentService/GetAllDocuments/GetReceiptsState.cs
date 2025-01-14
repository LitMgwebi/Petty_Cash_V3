namespace Backend.Services.DocumentService.GetAllDocuments
{
    public class GetReceiptsState(BackendContext db) : IIndexDocuments
    {
        private BackendContext db = db;

        public async Task<ServerResponse<IEnumerable<Document>>> GetDocuments()
        {
            IEnumerable<Document> documents = await db.Documents
               .OfType<Receipt>()
               .Where(x => x.IsActive == true)
               .AsNoTracking()
               .ToListAsync();

            if (documents == null)
                return new ServerResponse<IEnumerable<Document>>
                {
                    Success = false,
                    Message = "System could not retrieve any receipts."
                };

            return new ServerResponse<IEnumerable<Document>>
            {
                Success = true,
                Data = documents,
                Message = "Receipts retrieved successfully."
            };
        }
    }
}
