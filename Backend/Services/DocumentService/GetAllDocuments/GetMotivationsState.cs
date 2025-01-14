namespace Backend.Services.DocumentService.GetAllDocuments
{
    public class GetMotivationsState(BackendContext db) : IIndexDocuments
    {
        private BackendContext db = db;

        public async Task<ServerResponse<IEnumerable<Document>>> GetDocuments()
        {
            IEnumerable<Document> documents = await db.Documents
               .OfType<Motivation>()
               .Where(x => x.IsActive == true)
               .AsNoTracking()
               .ToListAsync();

            if (documents == null)
                return new ServerResponse<IEnumerable<Document>>
                {
                    Success = false,
                    Message = "System could not retrieve any motivations."
                };

            return new ServerResponse<IEnumerable<Document>>
            {
                Success = true,
                Data = documents,
                Message = "Motivations retrieved successfully."
            };
        }
    }
}
