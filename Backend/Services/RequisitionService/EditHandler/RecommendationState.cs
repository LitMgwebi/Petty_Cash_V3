namespace Backend.Services.RequisitionService.EditHandler
{
    public class RecommendationState(Requisition reviewRequisition, string userId) : IEditState
    {
        private readonly Requisition reviewRequisition = reviewRequisition;
        private string userId = userId;

        public async Task<ServerResponse<Requisition>> EditRequisition(BackendContext _db, Requisition requisition)
        {
            ServerResponse<Requisition> response = new();
            if (reviewRequisition.ManagerRecommendation == null)
            {
                requisition.ManagerId = userId;
                requisition.ManagerRecommendationDate = DateTime.Now;

                if (requisition.ManagerRecommendationId == Status.Rejected)
                {
                    requisition.Stage = "This requisition has been rejected.";
                    requisition.CloseDate = DateTime.Now;
                    requisition.State = null;
                    requisition.StateId = requisition.ManagerRecommendationId;
                    response.Message = "The rejection has been saved to system.";
                }
                else if (requisition.ManagerRecommendationId == Status.Recommended)
                {
                    requisition.Stage = "This requisition has been recommended. Awaiting Finance Approval.";
                    response.Message = "The recommendation has been saved to system.";
                }

                _db.Requisitions.Update(requisition);

                if (await _db.SaveChangesAsync() == 0)
                {
                    response.Success = false;
                    response.Message = $"System could not record the line manager proceessing for the requisition belonging to: {requisition.Applicant!.FullName}.";
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
                    Message = $"This requisition has already been reviewed by {reviewRequisition.Manager!.FullName}."
                };
            }
        }
    }
}
