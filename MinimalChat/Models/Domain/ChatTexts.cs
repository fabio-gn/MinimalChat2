using System.ComponentModel.DataAnnotations;

namespace MinimalChat.Models.Domain
{
    public class ChatTexts
    {
        [Key]
        public int Id { get; set; }
        public string TextMessage { get; set; }
        public string UserName { get; set; }
        public string RoomId { get; set; }

    }
}
