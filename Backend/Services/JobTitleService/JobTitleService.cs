namespace Backend.Services.JobTitleService
{
    public class JobTitleService(BackendContext db): IJobTitle
    {
        private readonly BackendContext _db = db;

        public async Task<ServerResponse<IEnumerable<JobTitle>>> GetJobTitles()
        {
            try
            {
                IEnumerable<JobTitle> jobTitles = await _db.JobTitles
                    .Where(x => x.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

                return jobTitles == null ?
                    new ServerResponse<IEnumerable<JobTitle>>
                    {
                        Success = false,
                        Message = "System could not retrieve job titles."
                    } :
                    new ServerResponse<IEnumerable<JobTitle>>
                    {
                        Success = true,
                        Data = jobTitles,
                        Message = $"Job titles have been retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<JobTitle>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<JobTitle>> GetJobTitle(int id)
        {
            try
            {
                JobTitle jobTitle = await _db.JobTitles
                    .Where(x => x.IsActive == true)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.JobTitleId == id);

                return jobTitle == null ?
                    new ServerResponse<JobTitle>
                    {
                        Success = false,
                        Message = "System could not retrieve job title."
                    } :
                    new ServerResponse<JobTitle>
                    {
                        Success = true,
                        Data = jobTitle,
                        Message = $"{jobTitle.Description} has been retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<JobTitle>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

    }
}
