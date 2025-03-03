namespace Backend.Services.VaultService
{
    public class VaultService(BackendContext db) : IVault
    {
        private BackendContext _db = db;


        public async Task<ServerResponse<IEnumerable<Vault>>> GetVaults()
        {
            try
            {
                IEnumerable<Vault> vaults = await _db.Vaults
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

                return vaults == null ?
                    new ServerResponse<IEnumerable<Vault>>
                    {
                        Success = false,
                        Message = "System could not find any vaults."
                    } :
                    new ServerResponse<IEnumerable<Vault>>
                    {
                        Success = true,
                        Data = vaults,
                        Message = "Vaults retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Vault>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Vault>> GetVault(int vaultId)
        {
            try
            {
                Vault? vault = await _db.Vaults
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.VaultId == vaultId);

                return vault == null ?
                    new ServerResponse<Vault>
                    {
                        Success = false,
                        Message = "System could not retrieve vault."
                    } :
                    new ServerResponse<Vault>
                    {
                        Success = true,
                        Data = vault,
                        Message = $"Vault retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Vault>
                {
                    Success = false,
                    Message = ex.Message
                };
            }

        }

        public async Task<ServerResponse<Vault>> CreateVault(Vault vault)
        {
            try
            {
                await _db.Vaults.AddAsync(vault);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Vault>
                    {
                        Success = false,
                        Message = "System could not add the new vault."
                    } :
                    new ServerResponse<Vault>
                    {
                        Success = true,
                        Data = vault,
                        Message = $"Vault added successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Vault>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Vault>> EditVault(Vault vault)
        {

            try
            {
                _db.Vaults.Update(vault);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Vault>
                    {
                        Success = false,
                        Message = "System could not update the new vault."
                    } :
                    new ServerResponse<Vault>
                    {
                        Success = true,
                        Data = vault,
                        Message = $"Vault added successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Vault>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Vault>> DeleteVault(Vault vault)
        {

            try
            {
                vault.IsActive = false;
                _db.Vaults.Update(vault);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Vault>
                    {
                        Success = false,
                        Message = $"System was unable to delete vault #{vault.VaultId}."
                    } :
                    new ServerResponse<Vault>
                    {
                        Success = true,
                        Message = $"Vault #{vault.VaultId} deleted successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Vault>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
