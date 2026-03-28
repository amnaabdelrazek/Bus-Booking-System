using Microsoft.AspNetCore.SignalR;

namespace Bus_Booking_System.Hubs
{
    public class DashboardHub : Hub
    {
        
        public async Task UpdateDashboardStats()
        {
            await Clients.All.SendAsync("ReceiveStatsUpdate");
        }
    }
}