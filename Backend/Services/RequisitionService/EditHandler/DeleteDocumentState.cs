namespace Backend.Services.RequisitionService.EditHandler
{
    public class DeleteDocumentState(string type) : IEditState
    {
        public string type = type;

        public async Task<ServerResponse<Requisition>> EditRequisition(BackendContext _db, Requisition requisition)
        {
            if (type == Requisition.editStates.DeleteMotivation)
            {
                if (!requisition.Documents.OfType<Motivation>().Where(g => g.IsActive).Any())
                {
                    requisition.Stage = "Requisition has been stored in the system. Motivation must be uploaded before it can be sent for recommendation.";
                    _db.Requisitions.Update(requisition);

                    return await _db.SaveChangesAsync() == 0 ?
                        new ServerResponse<Requisition>
                        {
                            Success = false,
                            Message = " but system could not update requisition"
                        } :
                        new ServerResponse<Requisition>
                        {
                            Success = true,
                            Message = " and requisition has been updated"
                        };
                }

                return new ServerResponse<Requisition>
                {
                    Success = true,
                    Message = "...."
                };
            }
            else if (type == Requisition.editStates.DeleteReceipt)
            {
                if (!requisition.Documents.OfType<Receipt>().Where(g => g.IsActive).Any())
                {
                    requisition.ReceiptReceived = false;
                    requisition.Stage = "Petty Cash has been issued. Please bring back change and upload receipt as soon as possible.";

                    _db.Requisitions.Update(requisition);
                    return await _db.SaveChangesAsync() == 0 ?
                       new ServerResponse<Requisition>
                       {
                           Success = false,
                           Message = " but system could not update requisition"
                       } :
                       new ServerResponse<Requisition>
                       {
                           Success = true,
                           Message = " and requisition has been updated"
                       };
                }
                return new ServerResponse<Requisition>
                {
                    Success = true,
                    Message = "...."
                };
            }
            else
            {
                return new ServerResponse<Requisition>
                {
                    Success = true,
                    Message = "System requires correct command to download document."
                };
            }
        }
    }
}
