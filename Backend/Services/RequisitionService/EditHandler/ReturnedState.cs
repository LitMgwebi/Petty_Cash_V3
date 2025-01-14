namespace Backend.Services.RequisitionService.EditHandler
{
    public class ReturnedState(Requisition reviewReq) : IEditState
    {
        private Requisition reviewRequisition = reviewReq;
        public async Task<ServerResponse<Requisition>> EditRequisition(BackendContext _db, Requisition requisition)
        {
            if (reviewRequisition.ReceiptReceived == true)
            {
                if (requisition.TotalExpenses <= requisition.CashIssued)
                {
                    requisition.Change = requisition.CashIssued - requisition.TotalExpenses;
                    requisition.Stage = "The receipts and total expenses for this requisition have been recorded. Please provide change to Accounts Payable.";
                    requisition.State = null;
                    requisition.StateId = Status.Returned;
                    requisition.ReturnedDate = DateTime.Now;

                    _db.Requisitions.Update(requisition);
                    int result = await _db.SaveChangesAsync();

                    return await _db.SaveChangesAsync() == 0 ?
                         new ServerResponse<Requisition>
                         {
                             Success = false,
                             Message = "System could not record the return of the change and receipts. Please contact ICT."
                         } :
                         new ServerResponse<Requisition>
                         {
                             Success = true,
                             Message = "Total expenses and receipt/s have been recorded. Please provide change to Accounts Payable."
                         };
                }
                else
                {
                    return new ServerResponse<Requisition>
                    {
                        Success = false,
                        Message = "The amount entered is higher than issued cash. Please reenter amount."
                    };
                }
            }
            else
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = "You have no uploaded receipts. Please upload receipt before you submitting."
                };
        }
    }
}
