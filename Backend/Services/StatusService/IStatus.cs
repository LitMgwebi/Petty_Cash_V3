namespace Backend.Services.StatusService
{
    public interface IStatus
    {
        public Task<ServerResponse<IEnumerable<Status>>> GetStatuses();
        public Task<ServerResponse<IEnumerable<Status>>> GetApprovedStatuses();
        public Task<ServerResponse<IEnumerable<Status>>> GetRecommendedStatuses();
        public Task<ServerResponse<IEnumerable<Status>>> GetRequisitionStatesStatuses();
        public Task<ServerResponse<Status>> GetStatus(int id);
    }
}
