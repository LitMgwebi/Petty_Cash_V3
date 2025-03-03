namespace Backend.Services.OfficeService
{
    public class OfficeService(BackendContext db) : IOffice
    {
        private BackendContext _db = db;

        public async Task<ServerResponse<IEnumerable<Office>>> GetOffices()
        {
            try
            {
                IEnumerable<Office> offices = await _db.Offices
                    .Where(x => x.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

                return offices == null ?
                    new ServerResponse<IEnumerable<Office>>
                    {
                        Success = false,
                        Message = "System could not retrieve any offices."
                    } :
                    new ServerResponse<IEnumerable<Office>>
                    {
                        Success = true,
                        Data = offices,
                        Message = "Offices retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Office>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Office>> GetOffice(int id)
        {
            try
            {
                Office? office = await _db.Offices
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.OfficeId == id);

                return office == null ?
                    new ServerResponse<Office>
                    {
                        Success = false,
                        Message = "System could not retrieve Office."
                    } :
                    new ServerResponse<Office>
                    {
                        Success = true,
                        Data = office,
                        Message = $"{office.Name} retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Office>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Office>> CreateOffice(Office office)
        {
            try
            {
                await _db.Offices.AddAsync(office);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Office>
                    {
                        Success = false,
                        Message = "System was unable to add the new office."
                    } :
                    new ServerResponse<Office>
                    {
                        Success = true,
                        Data = office,
                        Message = $"{office.Name} added to database successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Office>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Office>> EditOffice(Office office)
        {
            try
            {
                _db.Offices.Update(office);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Office>
                    {
                        Success = false,
                        Message = $"System was unable to update ${office.Name}."
                    } :
                    new ServerResponse<Office>
                    {
                        Success = true,
                        Data = office,
                        Message = $"{office.Name} updated successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Office>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Office>> DeleteOffice(Office office)
        {
            try
            {
                office.IsActive = false;
                _db.Offices.Update(office);
                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Office>
                    {
                        Success = false,
                        Message = $"System was unable to delete ${office.Name}."
                    } :
                    new ServerResponse<Office>
                    {
                        Success = true,
                        Data = office,
                        Message = $"{office.Name} deleted successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Office>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
