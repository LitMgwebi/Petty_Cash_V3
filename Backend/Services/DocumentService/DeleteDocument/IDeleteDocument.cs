using Backend.Models;
using Backend.Services.RequisitionService;

namespace Backend.Services.DocumentService.DeleteDocument
{
    public interface IDeleteDocument
    {
        Task<ServerResponse<Document>> DeleteDocument(BackendContext db, IRequisition _requisition, Document document);
    }
}
