namespace Backend.Services.RequisitionService.EditHandler
{
    public class AddDocumentState : IEditState
    {
        private string type;
        public AddDocumentState(string type)
        {
            this.type = type;
        }

        public async Task<ServerResponse<Requisition>> EditRequisition(BackendContext _db, Requisition requisition)
        {
            if (type == Document.Motivation)
            {
                requisition.Stage = "Motivation has been uploaded. Requisition has been sent for recommendation.";
                _db.Requisitions.Update(requisition);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Requisition>
                    {
                        Success = false,
                        Message = $"System could not update requisition #{requisition.RequisitionId} to acknowledge the motivation being uploaded."
                    } :
                    new ServerResponse<Requisition>
                    {
                        Success = true,
                        Message = " and requisition details have been updated with motivation information."
                    };
            }
            else if (type == Document.Receipt)
            {
                requisition.Stage = "Receipt has been uploaded.";
                requisition.ReceiptReceived = true;

                _db.Requisitions.Update(requisition);
                return await _db.SaveChangesAsync() == 0 ?
                   new ServerResponse<Requisition>
                   {
                       Success = false,
                       Message = $"System could not update requisition #{requisition.RequisitionId} to acknowledge the receipt/s being uploaded."
                   } :
                   new ServerResponse<Requisition>
                   {
                       Success = true,
                       Message = " and requisition details have been updated with receipt information."
                   };
            }
            else
            {
                return new ServerResponse<Requisition>
                {
                    Success = true,
                    Message = "System requires correct command to upload document."
                };
            }
        }
    }
}
