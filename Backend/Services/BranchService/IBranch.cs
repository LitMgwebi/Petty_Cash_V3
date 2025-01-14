namespace Backend.Services.BranchService
{
    public interface IBranch
    {
        public Task<ServerResponse<IEnumerable<Branch>>> GetBranches();
        public Task<ServerResponse<Branch>> GetBranch(int id);
        public Task<ServerResponse<Branch>> CreateBranch(Branch Branch);
        public Task<ServerResponse<Branch>> EditBranch(Branch Branch);
        public Task<ServerResponse<Branch>> DeleteBranch(Branch Branch);
    }
}
