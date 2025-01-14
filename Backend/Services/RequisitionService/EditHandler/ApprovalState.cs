namespace Backend.Services.RequisitionService.EditHandler
{
    public class ApprovalState : IEditState
    {
        private readonly Requisition reviewRequisition;
        private string userId;
        public ApprovalState(Requisition reviewRequisition, string userId)
        {
            this.reviewRequisition = reviewRequisition;
            this.userId = userId;
        }

        public async Task<ServerResponse<Requisition>> EditRequisition(BackendContext _db, Requisition requisition)
        {
            ServerResponse<Requisition> response = new();
            if (reviewRequisition.FinanceApproval == null)
            {
                Random code = new Random();
                requisition.FinanceOfficerId = userId;
                requisition.FinanceApprovalDate = DateTime.Now;

                if (requisition.FinanceApprovalId == Status.Approved)
                {
                    requisition.ApplicantCode = code.Next(10000, 99999);
                    requisition.StateId = Status.Open;
                    requisition.Stage = "Finance has approved this requisition. Go to Finance to retrieve the petty cash. Don't forget your applicant code.";
                    response.Message = "The approval has been saved to the system.";
                }
                else if (requisition.FinanceApprovalId == Status.Declined)
                {
                    requisition.Stage = "Finance has declined this requisition.";
                    requisition.CloseDate = DateTime.Now;
                    requisition.State = null;
                    requisition.StateId = requisition.FinanceApprovalId;
                    response.Message = "The denial has been saved to the system.";
                }

                _db.Requisitions.Update(requisition);

                if (await _db.SaveChangesAsync() == 0)
                {
                    response.Success = false;
                    response.Message = $"System could not record the finance proceessing for the requisition belonging to: {requisition.Applicant!.FullName}.";
                }
                else
                {
                    response.Success = true;
                }
                return response;
            }
            else
            {
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = $"This requisition has already been reviewed by {reviewRequisition.FinanceOfficer!.FullName}."
                };
            }
        }
    }
}
