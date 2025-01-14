namespace Backend.Services.AccountSetService
{
    public class AccountSetService(BackendContext db): IAccountSet
    {
        private BackendContext _db = db;

        public async Task<ServerResponse<IEnumerable<AccountSet>>> GetAllAccountSets(string command)
        {
            try
            {
                IEnumerable<AccountSet> accounts = new List<AccountSet>();
                if (command == AccountSet.MainAccount)
                {
                    accounts = await _db.AccountSets
                       .OfType<MainAccount>()
                       .Where(x => x.IsActive == true)
                       .OrderBy(x => x.Name)
                       .AsNoTracking()
                       .ToListAsync();
                }
                else if (command == AccountSet.SubAccount)
                {
                    accounts = await _db.AccountSets
                        .OfType<SubAccount>()
                        .Where(x => x.IsActive == true)
                        .OrderBy(x => x.Name)
                        .AsNoTracking()
                        .ToListAsync();
                }
                else
                {
                    return new ServerResponse<IEnumerable<AccountSet>>
                    {
                        Success = false,
                        Message = "Server could not recognise command in retrieving account sets."
                    };
                }

                if (accounts == null)
                {
                    return new ServerResponse<IEnumerable<AccountSet>>
                    {
                        Success = false,
                        Message = "Server could not resolve account sets."
                    };
                }

                return new ServerResponse<IEnumerable<AccountSet>>
                {
                    Success = true,
                    Data = accounts,
                    Message = "Accounts retrieved successfully from database."
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<AccountSet>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<AccountSet>> GetOneAccountSet(string command, int id)
        {
            try
            {
                AccountSet account = new AccountSet();
                if (command == AccountSet.MainAccount)
                {
                    account = await _db.AccountSets
                        .OfType<MainAccount>()
                        .Where(a => a.IsActive == true)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.AccountSetId == id);
                }
                else if (command == AccountSet.SubAccount)
                {
                    account = await _db.AccountSets
                        .OfType<SubAccount>()
                        .Where(a => a.IsActive == true)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.AccountSetId == id);
                }
                else
                {
                    return new ServerResponse<AccountSet>
                    {
                        Success = false,
                        Message = "Server could not recognise command in retrieving account set."
                    };
                }

                if (account == null)
                {
                    return new ServerResponse<AccountSet>
                    {
                        Success = false,
                        Message = "Server could not resolve account set."
                    };
                }

                return new ServerResponse<AccountSet>
                {
                    Success = true,
                    Data = account,
                    Message = $"{account.Name} retrieved successfully from database."
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<AccountSet>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<AccountSet>> CreateAccountSet(AccountSet account, string command)
        {
            try
            {
                if (command == AccountSet.MainAccount)
                {
                    ServerResponse<IEnumerable<AccountSet>> mainAccResponses = await GetAllAccountSets(AccountSet.MainAccount);
                    IEnumerable<AccountSet> mainAccs = mainAccResponses.Data!;

                    if (mainAccs.Select(x => x.Name).ToList().Contains(account.Name))
                    {
                        return new ServerResponse<AccountSet>
                        {
                            Success = false,
                            Message = $"System already contains Main Account with the name: {account.Name}"
                        };
                    }
                    if (mainAccs.Select(x => x.AccountNumber).ToList().Contains(account.AccountNumber))
                    {
                        return new ServerResponse<AccountSet>
                        {
                            Success = false,
                            Message = $"System already contains Main Account with the number: {account.AccountNumber}"
                        };
                    }
                }
                else if (command == AccountSet.SubAccount)
                {
                    ServerResponse<IEnumerable<AccountSet>> subAccResponses = await GetAllAccountSets(AccountSet.SubAccount);
                    IEnumerable<AccountSet> subAccs = subAccResponses.Data!;

                    if (subAccs.Select(x => x.Name).ToList().Contains(account.Name))
                    {
                        return new ServerResponse<AccountSet>
                        {
                            Success = false,
                            Message = $"System already contains Sub Account with the name: {account.Name}"
                        };
                    }
                    if (subAccs.Select(x => x.AccountNumber).ToList().Contains(account.AccountNumber))
                    {
                        return new ServerResponse<AccountSet>
                        {
                            Success = false,
                            Message = $"System already contains Sub Account with the number: {account.AccountNumber}"
                        };
                    }
                }
                else
                {
                    return new ServerResponse<AccountSet>
                    {
                        Success = false,
                        Message = "Server could not recognise command in validiating account."
                    };
                }

                await _db.AccountSets.AddAsync(account);

                if (await _db.SaveChangesAsync() == 0)
                {
                    return new ServerResponse<AccountSet>
                    {
                        Success = false,
                        Message = "System could not save account."
                    };
                }

                return new ServerResponse<AccountSet>
                {
                    Success = true,
                    Message = $"{account.Name} was able to be saved."
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<AccountSet>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<AccountSet>> EditAccountSet(AccountSet accountSet)
        {
            try
            {
                _db.AccountSets.Update(accountSet);
                if (await _db.SaveChangesAsync() == 0)
                {
                    return new ServerResponse<AccountSet>
                    {
                        Success = false,
                        Message = $"System could not edit {accountSet.Name}."
                    };
                }

                return new ServerResponse<AccountSet>
                {
                    Success = true,
                    Message = $"{accountSet.Name} was able to be updated."
                };

            }
            catch (Exception ex)
            {
                return new ServerResponse<AccountSet>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<AccountSet>> DeleteAccountSet(AccountSet accountSet)
        {
            try
            {
                accountSet.IsActive = false;
                _db.AccountSets.Update(accountSet);
                if (await _db.SaveChangesAsync() == 0)
                {
                    return new ServerResponse<AccountSet>
                    {
                        Success = false,
                        Message = $"System could not delete {accountSet.Name}."
                    };
                }

                return new ServerResponse<AccountSet>
                {
                    Success = true,
                    Message = $"{accountSet.Name} was deleted."
                };

            }
            catch (Exception ex)
            {
                return new ServerResponse<AccountSet>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
