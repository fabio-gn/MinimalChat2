using Microsoft.AspNetCore.SignalR;

namespace MinimalChat
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task JoinRoom(string roomName, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", $"<b>{username}</b> has joined the room {roomName}");
        }

        public async Task LeaveRoom(string roomName, string username)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", $"<b>{username}</b> has left the room {roomName}");

        }

        public async Task SendMessageToRoom(string roomName, string message, string username)
        {
            await Clients.Group(roomName).SendAsync("ReceiveMessage", $"{username}: {message}");
            await Clients.Group(roomName).SendAsync("SvuotaInput");
        }
    }
}
