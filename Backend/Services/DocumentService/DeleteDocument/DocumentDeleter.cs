using Backend.Models;
using Backend.Services.RequisitionService;

namespace Backend.Services.DocumentService.DeleteDocument
{
    public class DocumentDeleter
    {
        private IDeleteDocument state = null!;

        public void setState(IDeleteDocument state) { this.state = state; }

        public async Task<ServerResponse<Document>> request(BackendContext db, IRequisition _requisition, Document document) => await state.DeleteDocument(db, _requisition, document);
    }
}
