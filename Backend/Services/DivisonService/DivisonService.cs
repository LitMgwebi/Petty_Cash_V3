namespace Backend.Services.DivisonService
{
    public class DivisonService(BackendContext db): IDivision
    {
        private BackendContext _db = db;

        public async Task<ServerResponse<IEnumerable<Division>>> GetDivisions()
        {
            try
            {
                IEnumerable<Division> divisions = await _db.Divisions
                    .Where(x => x.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

                if (divisions == null)
                    return new ServerResponse<IEnumerable<Division>>
                    {
                        Success = false,
                        Message = "System could not retrieve any divisions."
                    };

                return new ServerResponse<IEnumerable<Division>>
                {
                    Success = true,
                    Data = divisions,
                    Message = "System could not retrieve any divisions."
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Division>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Division>> GetDivision(int id)
        {
            try
            {
                Division? division = await _db.Divisions
                    .Where(a => a.IsActive == true)
                    .Include(d => d.Department)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.DivisionId == id);

                if (division == null) return new ServerResponse<Division>
                {
                    Success = false,
                    Message = $"System could not retrieve any divisions with id of #{id}."
                };

                return new ServerResponse<Division>
                {
                    Success = true,
                    Data = division,
                    Message = $"{division.Name} retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Division>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Division>> CreateDivision(Division division)
        {
            try
            {
                await _db.Divisions.AddAsync(division);

                if (await _db.SaveChangesAsync() == 0)
                    return new ServerResponse<Division>
                    {
                        Success = false,
                        Message = $"System could not save division."
                    };

                return new ServerResponse<Division>
                {
                    Success = true,
                    Message = $"{division.Name} was able to be saved."
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Division>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Division>> EditDivision(Division division)
        {
            try
            {
                _db.Divisions.Update(division);
                if (await _db.SaveChangesAsync() == 0)
                    return new ServerResponse<Division>
                    {
                        Success = false,
                        Message = $"System could not edit {division.Name}."
                    };

                return new ServerResponse<Division>
                {
                    Success = true,
                    Message = $"{division.Name} was able to be edited."
                };
            }
            catch (Exception ex) { return new ServerResponse<Division> { Success = false, Message = ex.Message }; }
        }

        public async Task<ServerResponse<Division>> DeleteDivision(Division division)
        {
            try
            {
                division.IsActive = false;
                _db.Divisions.Update(division);
                if (await _db.SaveChangesAsync() == 0)
                    return new ServerResponse<Division>
                    {
                        Success = false,
                        Message = $"System could not delete {division.Name}."
                    };

                return new ServerResponse<Division>
                {
                    Success = true,
                    Message = $"{division.Name} was able to be deleted."
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Division>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
