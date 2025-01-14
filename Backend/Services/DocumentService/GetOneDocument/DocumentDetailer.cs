namespace Backend.Services.DocumentService.GetOneDocument
{
    public class DocumentDetailer
    {
        private IGetDocument state = null!;

        public void setState(IGetDocument state) => this.state = state;

        public async Task<ServerResponse<Document>> GetDocument() => await state.GetOneDocument();
    }
}
