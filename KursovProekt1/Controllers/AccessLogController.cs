using KursovProekt1.Data;
using KursovProekt1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KursovProekt1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccessLogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccessLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista sa logove
        public IActionResult Index()
        {
            var logs = _context.AccessLogs
                .Include(a => a.User)
                .Include(a => a.Room)
                .ToList();

            return View(logs);
        }

        // Forma za dobavqne
        public IActionResult Create()
        {
            ViewBag.Rooms = _context.Rooms.ToList();
            ViewBag.Users = _context.Users.ToList();
            return View();
        }

        // Dobavqne na log
        [HttpPost]
        public IActionResult Create(string userId, int roomId, string status)
        {
            var log = new AccessLog
            {
                UserId = userId,
                RoomId = roomId,
                EntryTime = DateTime.Now,
                Status = status
            };

            _context.AccessLogs.Add(log);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}