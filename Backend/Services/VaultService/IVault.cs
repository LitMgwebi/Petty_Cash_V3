namespace Backend.Services.VaultService
{
    public interface IVault
    {
        public Task<ServerResponse<IEnumerable<Vault>>> GetVaults();
        public Task<ServerResponse<Vault>> GetVault(int vaultId);
        public Task<ServerResponse<Vault>> CreateVault(Vault vault);
        public Task<ServerResponse<Vault>> EditVault(Vault vault);
        public Task<ServerResponse<Vault>> DeleteVault(Vault vault);
    }
}
