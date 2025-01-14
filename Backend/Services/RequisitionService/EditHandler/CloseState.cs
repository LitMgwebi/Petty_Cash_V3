using Backend.Services.TransactionService;

namespace Backend.Services.RequisitionService.EditHandler
{
    public class CloseState(ITransaction transaction) : IEditState
    {
        private ITransaction _transaction = transaction;

        public async Task<ServerResponse<Requisition>> EditRequisition(BackendContext _db, Requisition requisition)
        {
            if (requisition.TotalExpenses <= requisition.CashIssued)
            {
                requisition.CloseDate = DateTime.Now;
                requisition.ConfirmChangeReceived = true;
                requisition.StateId = Status.Closed;
                requisition.Stage = "Change has been brought back to Accounts Payable and requisition is closed.";

                if (requisition.Change > 0)
                    await _transaction.CreateTransaction((decimal)requisition.Change!, Transaction.Change, requisition.RequisitionId, $"{requisition.Applicant!.FullName} has brought back the change for their petty cash requisition.");

                _db.Requisitions.Update(requisition);

                return await _db.SaveChangesAsync() == 0 ?
                        new ServerResponse<Requisition>
                        {
                            Success = false,
                            Message = $"System could not close the requisition: {requisition.RequisitionId} made by {requisition.Applicant!.FullName} "
                        } :
                        new ServerResponse<Requisition>
                        {
                            Success = true,
                            Message = "Requisition has been closed."
                        };
            }
            else
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = "You entered in expenses that are higher than the issued cash. Please reenter amount."
                };
        }
    }
}

