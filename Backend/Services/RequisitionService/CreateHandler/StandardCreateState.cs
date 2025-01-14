namespace Backend.Services.RequisitionService.CreateHandler
{
    public class StandardCreateState : ICreateState
    {
        public async Task<ServerResponse<Requisition>> CreateRequisition(BackendContext _db, Requisition requisition, string userId)
        {
            requisition.Stage = "Requisiton has been sent for recommendation.";
            requisition.ApplicantId = userId;
            requisition.StartDate = DateTime.Now;
            requisition.StateId = Status.InProcess;
            requisition.NeedsMotivation = false;

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
                        Message = "The new Requisition has been added to the system."
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
