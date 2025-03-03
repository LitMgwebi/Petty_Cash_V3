namespace Backend.Services.BranchService
{
    public class BranchService(BackendContext db) : IBranch
    {
        private BackendContext _db = db;

        public async Task<ServerResponse<IEnumerable<Branch>>> GetBranches()
        {
            try
            {
                IEnumerable<Branch> Branchs = await _db.Branches
                    .Where(x => x.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

                return Branchs == null ?
                     new ServerResponse<IEnumerable<Branch>>
                     {
                         Success = false,
                         Message = "System could not retrieve any Branchs."
                     } :
                     new ServerResponse<IEnumerable<Branch>>
                     {
                         Success = true,
                         Data = Branchs,
                         Message = "Branches retrieved successfully."
                     };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Branch>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Branch>> GetBranch(int id)
        {
            try
            {
                Branch? Branch = await _db.Branches
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.BranchId == id);

                return Branch == null ?
                    new ServerResponse<Branch>
                    {
                        Success = false,
                        Message = "System could not retrieve Office."
                    } :
                    new ServerResponse<Branch>
                    {
                        Success = true,
                        Data = Branch,
                        Message = $"{Branch.Name} retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Branch>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Branch>> CreateBranch(Branch Branch)
        {
            try
            {
                await _db.Branches!.AddAsync(Branch);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Branch>
                    {
                        Success = false,
                        Message = "System was unable to add the new Branch."
                    } :
                    new ServerResponse<Branch>
                    {
                        Success = true,
                        Data = Branch,
                        Message = $"{Branch.Name} added to database successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Branch>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Branch>> EditBranch(Branch Branch)
        {
            try
            {
                _db.Branches!.Update(Branch);
                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Branch>
                    {
                        Success = false,
                        Message = $"System was unable to edit {Branch.Name}."
                    } :
                    new ServerResponse<Branch>
                    {
                        Success = true,
                        Data = Branch,
                        Message = $"{Branch.Name} updated successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Branch>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Branch>> DeleteBranch(Branch Branch)
        {
            try
            {
                Branch.IsActive = false;
                _db.Branches!.Update(Branch);
                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Branch>
                    {
                        Success = false,
                        Message = $"System was unable to delete {Branch.Name}."
                    } :
                    new ServerResponse<Branch>
                    {
                        Success = true,
                        Data = Branch,
                        Message = $"{Branch.Name} deleted successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Branch>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

    }
}
