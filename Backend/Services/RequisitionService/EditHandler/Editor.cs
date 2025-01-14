namespace Backend.Services.RequisitionService.EditHandler
{
    public class Editor
    {
        private IEditState state = null!;

        public void setState(IEditState state) => this.state = state;

        public async Task<ServerResponse<Requisition>> request(BackendContext db, Requisition requisition) => await state.EditRequisition(db, requisition);
    }
}
