using Backend.Services.RequisitionService;

namespace Backend.Services.DocumentService.DeleteDocument
{
    public class DeleteReceiptState : IDeleteDocument
    {
        public async Task<ServerResponse<Document>> DeleteDocument(BackendContext db, IRequisition _requisition, Document document)
        {
            ServerResponse<Requisition> requisitionResponse = await _requisition.GetRequisition(document.RequisitionId);
            Requisition requisition = requisitionResponse.Data!;

            if (requisition.StateId == Status.Issued || requisition.StateId == Status.Returned)
            {
                document.IsActive = false;
                db.Documents.Update(document);

                if (await db.SaveChangesAsync() == 0)
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = $"System could not delete ${document.FileName}.{document.FileExtension}."
                    };

                //TODO: code to edit the state of the requisition linked to this receipt goes here
                ServerResponse<Requisition> response = await _requisition.EditRequisition(requisition, Document.Receipt, forDoc: true, deleteDoc: true);

                if (response.Success == false)
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = $"Receipt was deleted, but requisition details were not changed."
                    };
                else
                    return new ServerResponse<Document>
                    {
                        Success = true,
                        Message = $"Receipt has successfully been deleted{response.Message}."
                    };
            }
            else
                return new ServerResponse<Document>
                {
                    Success = false,
                    Message = "You can only delete a receipt when the requisition is in an Issued or Returned state."
                };
        }
    }
}