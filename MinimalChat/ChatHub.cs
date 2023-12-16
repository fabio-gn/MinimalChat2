using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using MinimalChat.Data;
using MinimalChat.Models.Domain;

namespace MinimalChat
{
    public class ChatHub : Hub
    {
        //ServiceProvider mi permette di richiamare qui dentro il DbContext
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
            //Aggiungo al Gruppo (roomName) la connessione dello user che entra nella stanza
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            
            //Lo using con lo scope a quanto pare mi serve per poter richiamare qui dentro il DbContext
            using (var scope = _serviceProvider.CreateScope())
            {
                //Salvo il DbContext nella omonima variabile
                var DbContext = scope.ServiceProvider.GetRequiredService<MinimalChatDbContext>();

                //creo un nuovo oggetto textMessage di classe ChatTexts che verrà salvato nel DB
                ChatTexts textMessage = new ChatTexts();

                //Linq per prendermi i 15 messaggi più recenti salvati nel DB, in modo che l'utente appena arrivato possa vedere un po'
                //di messaggi vecchi.
                var lastMessages = DbContext.ChatTexts.Where(m => m.RoomId == roomName).OrderByDescending(m => m.Id).Take(15).ToList();

                //Ciclo la lista dei messaggi recenti per mandarli a schermo
                for(var i = lastMessages.Count - 1; i >= 0; i--)
                {
                    var message = lastMessages[i];
                    await Clients.Group(roomName).SendAsync("ReceiveMessage", $"<b>{message.UserName}</b>: {message.TextMessage}");
                }

                //Assegno all'oggetto textMessage tutte le proprietà: non è un messaggio di testo qualsiasi, ma quello che segnala
                //l'accesso di un utente nella stanza.
                textMessage.TextMessage = $"has joined the room {roomName}";
                textMessage.UserName = username;
                textMessage.RoomId = roomName;

                //aggiungo e salvo nel DB
                DbContext.ChatTexts.Add(textMessage);
                DbContext.SaveChanges();
            }
            await Clients.Group(roomName).SendAsync("ReceiveMessage", $"<b>{username}</b> has joined the room {roomName}");

        }

        public async Task LeaveRoom(string roomName, string username)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

            using (var scope = _serviceProvider.CreateScope())
            {
                var DbContext = scope.ServiceProvider.GetRequiredService<MinimalChatDbContext> ();
                ChatTexts textMessage = new ChatTexts();
                textMessage.RoomId = roomName;
                textMessage.UserName= username;
                textMessage.TextMessage= $"has left the room {roomName}";

                DbContext.ChatTexts.Add (textMessage);
                DbContext.SaveChanges();
            }
            
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
