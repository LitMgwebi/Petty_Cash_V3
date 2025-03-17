using Backend.Services.RequisitionService;

namespace Backend.Services.DocumentService.DeleteDocument
{
    public class DeleteMotivationState : IDeleteDocument
    {
        public async Task<ServerResponse<Document>> DeleteDocument(BackendContext db, IRequisition _requisition, Document document)
        {
            ServerResponse<Requisition> requisitionResponse = await _requisition.GetRequisition(document.RequisitionId);
            Requisition requisition = requisitionResponse.Data!;

            if (requisition.ManagerId == null)
            {
                document.IsActive = false;
                db.Documents.Update(document);

                if (await db.SaveChangesAsync() == 0)
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = $"System could not delete ${document.FileName}.{document.FileExtension}."
                    };

                //TODO: code to edit the state of the requisition linked to this motivation goes here
                ServerResponse<Requisition> response = await _requisition.EditRequisition(requisition, Requisition.editStates.DeleteMotivation);
                if (response.Success == false)
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = $"Motivation was deleted, but requisition details were not changed."
                    };
                else
                    return new ServerResponse<Document>
                    {
                        Success = true,
                        Message = $"Motivation has successfully been deleted{response.Message}."
                    };
            }
            else
                return new ServerResponse<Document>
                {
                    Success = false,
                    Message = "Your requisition has already been processed by your line manager. You cannot delete your motivation."
                };
        }
    }
}
