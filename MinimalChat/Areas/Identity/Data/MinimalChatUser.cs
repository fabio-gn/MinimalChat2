using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MinimalChat.Models.Domain;

namespace MinimalChat.Areas.Identity.Data;

// Add profile data for application users by adding properties to the MinimalChatUser class
public class MinimalChatUser : IdentityUser
{
    
    [Key]
    public int Id { get; set; }
    //[ForeignKey(nameof(Roles))]
    //public int RoleId { get; set; }
    //public Roles Roles { get; set; }

    //ELIMINARE, è la causa del bug che mostra email invece di nome
    //public string UserName { get; set; }
    
    //[ForeignKey(nameof(Rooms))]
    //public int RoomsId { get; set; }
    //public Rooms? Rooms { get; set; }
    public bool isHost { get; set; }

    public bool isGuest { get; set; } = false;
    

}

