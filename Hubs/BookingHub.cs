using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Bus_Booking_System.Hubs
{
    public class BookingHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _tempLockedSeats = new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentDictionary<string, int> _userConnections = new ConcurrentDictionary<string, int>();

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
                _userConnections.AddOrUpdate(userId, 1, (key, val) => val + 1);
            return base.OnConnectedAsync();
        }

        public async Task<bool> LockSeat(int tripId, string seatId)
        {
            string key = $"{tripId}_{seatId}";
            string userId = Context.UserIdentifier;

            if (string.IsNullOrEmpty(userId)) return false;

            if (_tempLockedSeats.TryAdd(key, userId))
            {
                // إبلاغ الآخرين ليتحول اللون عندهم للأصفر
                await Clients.Others.SendAsync("SeatLocked", tripId, seatId);
                return true;
            }
            return false;
        }

        public async Task UnlockSeat(int tripId, string seatId)
        {
            string key = $"{tripId}_{seatId}";
            if (_tempLockedSeats.TryRemove(key, out _))
            {
                // إبلاغ الجميع ليعود الكرسي أبيض
                await Clients.All.SendAsync("SeatUnlocked", tripId, seatId);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections.AddOrUpdate(userId, 0, (key, val) => Math.Max(0, val - 1));
                await Task.Delay(5000); // مهلة بسيطة

                if (_userConnections.TryGetValue(userId, out int count) && count == 0)
                {
                    var userLocks = _tempLockedSeats.Where(x => x.Value == userId).ToList();
                    foreach (var lockItem in userLocks)
                    {
                        if (_tempLockedSeats.TryRemove(lockItem.Key, out _))
                        {
                            var parts = lockItem.Key.Split('_');
                            await Clients.All.SendAsync("SeatUnlocked", int.Parse(parts[0]), parts[1]);
                        }
                    }
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        public static void ReleaseSeat(int tripId, string seatId)
        {
            _tempLockedSeats.TryRemove($"{tripId}_{seatId}", out _);
        }
    }
}