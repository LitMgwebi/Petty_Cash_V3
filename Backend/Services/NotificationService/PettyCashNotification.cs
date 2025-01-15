using Backend.Models;
using Backend.Services.RequisitionService;

namespace Backend.Services.NotificationService
{
    public class PettyCashNotification : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        //private readonly IMailService _email;

        public PettyCashNotification(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            //while (!cancellationToken.IsCancellationRequested)
            //{
            //    if (IsWeekday() && IsWithinWorkingHours())
            //    {
            //        using (var scope = serviceScopeFactory.CreateScope())
            //        {
            //            IRequisition _requisition = scope.ServiceProvider.GetRequiredService<IRequisition>();

            //            ServerResponse<List<Requisition>> requisitionResponse = await _requisition.GetRequisitions("tracking");
            //            List<Requisition> requisitions = requisitionResponse.Data!;

            //            if(requisitions != null)
            //            {
            //                foreach (var requisition in requisitions)
            //                {
            //                    if (requisition.ConfirmChangeReceived == false && requisition.IssueDate != null)
            //                    {
            //                        DateTime issueDate = (DateTime)requisition.IssueDate;
            //                        DateTime dayAfterIssuing = issueDate.AddDays(1);
            //                        // What happens after a weekend. Will the code still add one day and spazz out?
            //                        if (dayAfterIssuing < DateTime.Now)
            //                        {
            //                            //email to tell user that 24 hrs has passed.

            //                            await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
            //                            // email a reminder to say that an hour has passed
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        await Task.Delay(TimeSpan.FromMinutes(15), cancellationToken);
            //    }
            //}
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