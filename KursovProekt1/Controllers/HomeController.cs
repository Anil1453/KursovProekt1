using KursovProekt1.Data;
using Microsoft.AspNetCore.Mvc;

namespace KursovProekt1.Controllers
{
    public class HomeController : Controller
    {
        // Vruzka s bazata danni
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Nachalna stranica s statistiki
        public IActionResult Index()
        {
            // Broi stai
            ViewBag.TotalRooms = _context.Rooms.Count();

            // Broi potrebiteli
            ViewBag.TotalUsers = _context.Users.Count();

            // Broi logove
            ViewBag.TotalLogs = _context.AccessLogs.Count();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}