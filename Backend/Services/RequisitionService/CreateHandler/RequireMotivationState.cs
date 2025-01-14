namespace Backend.Services.RequisitionService.CreateHandler
{
    public class RequireMotivationState : ICreateState
    {
        public async Task<ServerResponse<Requisition>> CreateRequisition(BackendContext _db, Requisition requisition, string userId)
        {
            requisition.Stage = "Requisition has been stored in the system. Motivation must be uploaded before it can be sent for recommendation.";
            requisition.ApplicantId = userId;
            requisition.StartDate = DateTime.Now;
            requisition.StateId = Status.InProcess;
            requisition.NeedsMotivation = true;

            try
            {
                await _db.Requisitions.AddAsync(requisition);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Requisition>
                    {
                        Success = false,
                        Message = "System could not add the new Requisition."
                    } :
                    new ServerResponse<Requisition>
                    {
                        Success = true,
                        Data = requisition,
                        Message = "The new requisition has been added to the system. Please upload motivation."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
