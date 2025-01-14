namespace Backend.Services.DivisonService
{
    public interface IDivision
    {
        public Task<ServerResponse<IEnumerable<Division>>> GetDivisions();
        public Task<ServerResponse<Division>> GetDivision(int id);
        public Task<ServerResponse<Division>> CreateDivision(Division division);
        public Task<ServerResponse<Division>> EditDivision(Division division);
        public Task<ServerResponse<Division>> DeleteDivision(Division division);
    }
}
