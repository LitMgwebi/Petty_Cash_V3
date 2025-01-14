namespace Backend.Services.DocumentService.GetAllDocuments
{
    public class DocumentIndexer
    {
        private IIndexDocuments state = null!;

        public void setState(IIndexDocuments state) => this.state = state;

        public async Task<ServerResponse<IEnumerable<Document>>> request()
        {
            return await state.GetDocuments();
        }
    }
}
