using Backend.Services.VaultService;

namespace Backend.Services.NotificationService
{
    public class VaultNotification : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        //private readonly IMailService _email;

        public VaultNotification(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (IsWeekday() && IsWithinWorkingHours())
                {
                    using (var scope = serviceScopeFactory.CreateScope())
                    {
                        IVault _vault = scope.ServiceProvider.GetRequiredService<IVault>();

                        ServerResponse<Vault> vaultResponse = await _vault.GetVault(1);
                        Vault vault = vaultResponse.Data!;

                        if (vault.CurrentAmount < 6500)
                        {
                            //vault.Note = $"Replenish vault before issuing. Current balance: R{vault.CurrentAmount}.";
                            //// ***** Send email to finance about the state of vault *****
                            //await _vault.Edit(vault);
                            System.Diagnostics.Debug.WriteLine("Money is low nigga.");
                        }
                        else
                        {
                            //vault.Note = "";
                            //// ***** Send email to finance about the state of vault *****
                            //await _vault.Edit(vault);
                        }
                    }
                }
            }
        }

        bool IsWeekday()
        {
            var today = DateTime.Today.DayOfWeek;
            return today != DayOfWeek.Saturday && today != DayOfWeek.Sunday;
        }
        // Helper method to check if it's within working hours (8:00 AM to 4:30 PM)
        bool IsWithinWorkingHours()
        {
            var now = DateTime.Now;
            var startOfWorkingHours = now.Date.AddHours(8).AddMinutes(0);
            var endOfWorkingHours = now.Date.AddHours(16).AddMinutes(30);
            return now >= startOfWorkingHours && now <= endOfWorkingHours;
        }
    }
}
