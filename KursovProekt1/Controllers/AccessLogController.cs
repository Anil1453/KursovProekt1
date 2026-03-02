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

        // Показва логовете, може да се филтрира по статус
        public IActionResult Index(string status)
        {
            var logs = _context.AccessLogs
                .Include(a => a.User)
                .Include(a => a.Room)
                .AsQueryable();

            // Ако е избран статус - филтрирай
            if (!string.IsNullOrEmpty(status))
            {
                logs = logs.Where(a => a.Status == status);
            }

            ViewBag.SelectedStatus = status;

            return View(logs.OrderByDescending(a => a.EntryTime).ToList());
        }
    }
}