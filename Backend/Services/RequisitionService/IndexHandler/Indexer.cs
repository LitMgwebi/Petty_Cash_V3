namespace Backend.Services.RequisitionService.IndexHandler
{
    public class Indexer
    {
        private IIndexState state = null!;

        public void setState(IIndexState state) => this.state = state;

        public async Task<ServerResponse<List<Requisition>>> request(BackendContext db) => await state.GetRequisitions(db);
    }
}
