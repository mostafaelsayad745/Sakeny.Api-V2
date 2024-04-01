using Microsoft.AspNetCore.SignalR;


public class NotificationHub : Hub
{
    string conn = "";
    public string GetConnectionId()
    {
        return conn;
    }

    public async Task SendNotification(string connectionId, string message)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
    }
}