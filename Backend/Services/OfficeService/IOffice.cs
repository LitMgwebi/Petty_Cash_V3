namespace Backend.Services.OfficeService
{
    public interface IOffice
    {
        public Task<ServerResponse<IEnumerable<Office>>> GetOffices();
        public Task<ServerResponse<Office>> GetOffice(int id);
        public Task<ServerResponse<Office>> CreateOffice(Office office);
        public Task<ServerResponse<Office>> EditOffice(Office office);
        public Task<ServerResponse<Office>> DeleteOffice(Office office);
    }
}
