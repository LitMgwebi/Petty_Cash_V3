namespace Backend.Services.RequisitionService.EditHandler
{
    public class ExpensesState : IEditState
    {
        public async Task<ServerResponse<Requisition>> EditRequisition(BackendContext _db, Requisition requisition)
        {
            if (requisition.TotalExpenses <= requisition.CashIssued)
            {
                requisition.Change = requisition.CashIssued - requisition.TotalExpenses;
                requisition.Stage = "Total Expenses has been recorded. Please upload the receipt/s.";

                _db.Requisitions.Update(requisition);

                return await _db.SaveChangesAsync() == 0 ?
                        new ServerResponse<Requisition>
                        {
                            Success = false,
                            Message = $"System could not add the expenses to requisition #{requisition.RequisitionId}."
                        } :
                        new ServerResponse<Requisition>
                        {
                            Success = true,
                            Message = "Expenses have been added to the requisition."
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

