namespace Backend.Services.StatusService
{
    public class StatusService(BackendContext db) : IStatus
    {
        private BackendContext _db = db;


        public async Task<ServerResponse<IEnumerable<Status>>> GetStatuses()
        {
            try
            {
                IEnumerable<Status> statuses = await _db.Statuses
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

                return statuses == null ?
                    new ServerResponse<IEnumerable<Status>>
                    {
                        Success = false,
                        Message = "System could not find any statuses."
                    } :
                    new ServerResponse<IEnumerable<Status>>
                    {
                        Success = true,
                        Data = statuses,
                        Message = "Statuses retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Status>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<IEnumerable<Status>>> GetApprovedStatuses()
        {
            try
            {
                IEnumerable<Status> statuses = await _db.Statuses
                    .Where(a => a.IsActive == true)
                    .Where(c => c.IsApproved == true)
                    .AsNoTracking()
                    .ToListAsync();

                return statuses == null ?
                    new ServerResponse<IEnumerable<Status>>
                    {
                        Success = false,
                        Message = "System could not find any statuses linked to approvals."
                    } :
                    new ServerResponse<IEnumerable<Status>>
                    {
                        Success = true,
                        Data = statuses,
                        Message = "Statuses linked to approvals retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Status>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<IEnumerable<Status>>> GetRecommendedStatuses()
        {
            try
            {
                IEnumerable<Status> statuses = await _db.Statuses
                    .Where(a => a.IsActive == true)
                    .Where(c => c.IsRecommended == true)
                    .AsNoTracking()
                    .ToListAsync();

                return statuses == null ?
                    new ServerResponse<IEnumerable<Status>>
                    {
                        Success = false,
                        Message = "System could not find any statuses linked to recommendations."
                    } :
                    new ServerResponse<IEnumerable<Status>>
                    {
                        Success = true,
                        Data = statuses,
                        Message = "Statuses linked to recommendations retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Status>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<IEnumerable<Status>>> GetRequisitionStatesStatuses()
        {
            try
            {
                IEnumerable<Status> statuses = await _db.Statuses
                    .Where(a => a.IsActive == true)
                    .Where(c => c.IsState == true)
                    .AsNoTracking()
                    .ToListAsync();

                return statuses == null ?
                    new ServerResponse<IEnumerable<Status>>
                    {
                        Success = false,
                        Message = "System could not find any statuses linked to requisitions."
                    } :
                    new ServerResponse<IEnumerable<Status>>
                    {
                        Success = true,
                        Data = statuses,
                        Message = "Statuses linked to requisitions retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Status>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Status>> GetStatus(int id)
        {
            try
            {
                Status status = await _db.Statuses
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.StatusId == id);

                return status == null ?
                     new ServerResponse<Status>
                     {
                         Success = false,
                         Message = "System could not find any statuss."
                     } :
                     new ServerResponse<Status>
                     {
                         Success = true,
                         Data = status,
                         Message = "Status retrieved successfully."
                     };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Status>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
