using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class ModifyFollowedListHub:Hub<IModifyFollowedClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ReceiveNotification($"Thank you connecting {Context.User?.Identity?.Name}");
           
            await base.OnConnectedAsync();
        }
    }
    public interface IModifyFollowedClient
    {
        Task ReceiveNotification(string message);
    }
}
