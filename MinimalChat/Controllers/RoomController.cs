using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalChat.Data;
using MinimalChat.Models.Domain;

namespace MinimalChat.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
       
        public RoomController(MinimalChatDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        private readonly MinimalChatDbContext dbContext;
        public IActionResult Enter(int id)
        {
            
            var ActualRoom = dbContext.Rooms.FirstOrDefault(r => r.Id == id);
            return View(ActualRoom);
        }
    }
}
