using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalChat.Data;
using MinimalChat.Models;
using MinimalChat.Models.Domain;
using System.Diagnostics;

namespace MinimalChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MinimalChatDbContext chatDbContext)
        {
            this.chatDbContext = chatDbContext;
            _logger = logger;
        }
        private readonly MinimalChatDbContext chatDbContext;

        public IActionResult Index()
        {
            List<Rooms> rooms = new List<Rooms>();
            rooms = chatDbContext.Rooms.ToList();
            ViewBag.Rooms = rooms;
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateRoom()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateRoom(Rooms room)
        {
            room.DateOfCreation = DateTime.Now;

            chatDbContext.Add(room);
            chatDbContext.SaveChanges();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}