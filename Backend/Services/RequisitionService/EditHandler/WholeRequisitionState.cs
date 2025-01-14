using Backend.Services.GLAccountService;

namespace Backend.Services.RequisitionService.EditHandler
{
    public class WholeRequisitionState(IGLAccount gLAccount, Requisition reviewReq) : IEditState
    {
        private IGLAccount _glAccount = gLAccount;
        private Requisition reviewReq = reviewReq;

        public async Task<ServerResponse<Requisition>> EditRequisition(BackendContext _db, Requisition requisition)
        {
            ServerResponse<Requisition> response = new();
            if (requisition.ManagerRecommendationId == null)
            {
                ServerResponse<Glaccount> glaccountResponse = await _glAccount!.GetGlAccount(requisition.GlaccountId);
                requisition.Glaccount = glaccountResponse.Data!;

                if (requisition.Glaccount.NeedsMotivation || requisition.AmountRequested >= 2000)
                {
                    requisition.NeedsMotivation = true;
                    if (!reviewReq.Documents.Any())
                    {
                        requisition.Stage = "Requisition has been stored in the system. Motivation must be uploaded before it can be sent for recommendation.";
                    }
                }
                else
                {
                    requisition.NeedsMotivation = false;
                    requisition.Stage = "Requisiton has been sent for recommendation.";
                }

                _db.Requisitions.Update(requisition);
                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Requisition>
                    {
                        Success = false,
                        Message = $"System could not edit the requisition for {requisition.Applicant!.FullName}."
                    } :
                    new ServerResponse<Requisition>
                    {
                        Success = true,
                        Message = "The requisition has been edited."
                    };
            }
            else
            {
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = "You cannot edit a requisition after recommendation."
                };
            }
        }
    }
}
