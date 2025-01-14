namespace Backend.Services.JobTitleService
{
    public interface IJobTitle
    {
        public Task<ServerResponse<IEnumerable<JobTitle>>> GetJobTitles();
        public Task<ServerResponse<JobTitle>> GetJobTitle(int id);
    }
}
