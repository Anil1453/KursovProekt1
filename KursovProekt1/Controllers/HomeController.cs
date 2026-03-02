using KursovProekt1.Data;
using Microsoft.AspNetCore.Mvc;

namespace KursovProekt1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Начална страница със статистики
        public IActionResult Index()
        {
            ViewBag.TotalRooms = _context.Rooms.Count();
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalLogs = _context.AccessLogs.Count();
            ViewBag.TotalRequests = _context.AccessRequests.Count();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}