namespace Backend.Services.RequisitionService.CreateHandler
{
    public class Creator
    {
        private ICreateState state = null!;

        public void setState(ICreateState state) => this.state = state;

        public async Task<ServerResponse<Requisition>> request(BackendContext db, Requisition requisition, string userId) => await state.CreateRequisition(db, requisition, userId);
    }
}
