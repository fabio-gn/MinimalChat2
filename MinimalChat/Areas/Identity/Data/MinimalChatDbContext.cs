using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinimalChat.Areas.Identity.Data;
using System.Data;
using MinimalChat.Models.Domain;

namespace MinimalChat.Data;

public class MinimalChatDbContext : IdentityDbContext<MinimalChatUser>
{
    public MinimalChatDbContext(DbContextOptions<MinimalChatDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Rooms> Rooms { get; set; }
    public DbSet<MinimalChatUser> Users { get; set; }
    //public DbSet<Roles> Roles { get; set; }
}
