using Backend.Services.TransactionService;

namespace Backend.Services.RequisitionService.EditHandler
{
    public class IssuingState(ITransaction transaction, string userId, int attemptCode) : IEditState
    {
        private string userId = userId;
        private ITransaction _transaction = transaction;
        private readonly int attemptCode = attemptCode;

        public async Task<ServerResponse<Requisition>> EditRequisition(BackendContext _db, Requisition requisition)
        {
            if (requisition.FinanceApprovalId == Status.Declined)
            {
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = "This requisition has already been declined by finance."
                };
            }
            else if (requisition.ManagerRecommendationId == Status.Rejected)
            {
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = "This requisition has already been rejected by their line manager."
                };
            }

            if (attemptCode != requisition.ApplicantCode)
            {
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = "Applicant code is incorrect. Please review your code then enter it again."
                };
            }


            if (requisition.CashIssued <= requisition.AmountRequested)
            {
                requisition.IssueDate = DateTime.Now;
                requisition.IssuerId = userId;
                requisition.ConfirmApplicantCode = true;
                requisition.StateId = Status.Issued;
                requisition.Stage = "Petty Cash has been issued. Please bring back change and upload receipt as soon as possible.";

                ServerResponse<Transaction> transactionResponse = await _transaction.CreateTransaction((decimal)requisition.CashIssued!, Transaction.Withdrawal, requisition.RequisitionId, $"Petty Cash has been issued to {requisition.Applicant!.FullName}");

                if (transactionResponse.Success)
                {
                    _db.Requisitions.Update(requisition);

                    if (await _db.SaveChangesAsync() == 0)
                    {
                        return new ServerResponse<Requisition>
                        {
                            Success = false,
                            Message = $"System could not record issuing for the requisition by: {requisition.Applicant!.FullName}."
                        };
                    }
                    else
                    {
                        return new ServerResponse<Requisition>
                        {
                            Success = false,
                            Message = $"Issuing of Petty Cash has taken place. {transactionResponse.Message}."
                        };
                    }
                }
                else
                {
                    return new ServerResponse<Requisition>
                    {
                        Success = false,
                        Message = $"System could not process withdrawal transaction for requisition #{requisition.RequisitionId}."
                    };
                }
            }
            else
            {
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = $"You cannot issue petty cash higher than the amount requested."
                };
            }
        }

    }
}