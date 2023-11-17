

using MinimalChat.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace MinimalChat.Models.Domain
{
    public class Rooms
    {
        [Key]
        public int Id { get; set; }
        public string RoomName { get; set;}
        public string RoomDescription { get; set;} = string.Empty;
        public int RoomMembersQuantity { get; set;}
        
        
        

        public ICollection<MinimalChatUser> users { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
