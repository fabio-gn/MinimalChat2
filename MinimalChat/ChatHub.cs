using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using MinimalChat.Data;
using MinimalChat.Models.Domain;

namespace MinimalChat
{
    public class ChatHub : Hub
    {
        private readonly IServiceProvider _serviceProvider;

        public ChatHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        

        public async Task SendMessage(string user, string message, string roomName)
        {
           
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            
            
        }
        public async Task JoinRoom(string roomName, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var DbContext = scope.ServiceProvider.GetRequiredService<MinimalChatDbContext>();
                ChatTexts textMessage = new ChatTexts();
                
                var lastMessages = DbContext.ChatTexts.Where(m => m.RoomId == roomName).OrderByDescending(m => m.Id).Take(15).ToList();
                for(var i = lastMessages.Count - 1; i >= 0; i--)
                {
                    var message = lastMessages[i];
                    await Clients.Group(roomName).SendAsync("ReceiveMessage", $"<b>{message.UserName}</b>: {message.TextMessage}");
                }
                textMessage.TextMessage = $"has joined the room {roomName}";
                textMessage.UserName = username;
                textMessage.RoomId = roomName;

                DbContext.ChatTexts.Add(textMessage);
                DbContext.SaveChanges();
            }
            await Clients.Group(roomName).SendAsync("ReceiveMessage", $"<b>{username}</b> has joined the room {roomName}");

        }

        public async Task LeaveRoom(string roomName, string username)
        {

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", $"<b>{username}</b> has left the room {roomName}");

        }

        public async Task SendMessageToRoom(string roomName, string message, string username)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var DbContext = scope.ServiceProvider.GetRequiredService<MinimalChatDbContext>();
                ChatTexts textMessage = new ChatTexts();
                textMessage.TextMessage = message;
                textMessage.UserName = username;
                textMessage.RoomId = roomName;

                DbContext.ChatTexts.Add(textMessage);
                DbContext.SaveChanges();
            }
            await Clients.Group(roomName).SendAsync("ReceiveMessage", $"<b>{username}</b>: {message}");
            await Clients.Group(roomName).SendAsync("SvuotaInput");
        }
    }
}
