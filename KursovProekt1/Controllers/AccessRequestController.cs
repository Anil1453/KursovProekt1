using KursovProekt1.Data;
using KursovProekt1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KursovProekt1.Controllers
{
    // Само влезли потребители могат да използват тези страници
    [Authorize]
    public class AccessRequestController : Controller
    {
        // Връзка с базата данни
        private readonly ApplicationDbContext _context;

        // Помага да разберем кой е влязъл в системата
        private readonly UserManager<ApplicationUser> _userManager;

        public AccessRequestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Показва всички заявки
        // Админ вижда всички, потребителят - само своите
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);

            List<AccessRequest> requests;

            if (User.IsInRole("Admin"))
            {
                // Вземи всички заявки от базата
                requests = await _context.AccessRequests
                    .Include(r => r.User)
                    .Include(r => r.Room)
                    .OrderByDescending(r => r.RequestDate)
                    .ToListAsync();
            }
            else
            {
                // Вземи само заявките на този потребител
                requests = await _context.AccessRequests
                    .Include(r => r.User)
                    .Include(r => r.Room)
                    .Where(r => r.UserId == currentUserId)
                    .OrderByDescending(r => r.RequestDate)
                    .ToListAsync();
            }

            return View(requests);
        }

        // Показва формата за нова заявка
        public IActionResult Create()
        {
            // Изпрати списъка със стаи към формата
            ViewBag.Rooms = _context.Rooms.Where(r => r.IsActive).ToList();
            return View();
        }

        // Запазва новата заявка
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int roomId, string reason)
        {
            // Вземи ID-то на текущия потребител
            var currentUserId = _userManager.GetUserId(User);

            // Създай новата заявка
            var request = new AccessRequest
            {
                UserId = currentUserId,
                RoomId = roomId,
                Reason = reason,
                RequestDate = DateTime.Now,
                Status = "Pending",
                AdminResponse = "",
                ApprovedByAdminId = ""
            };

            // Запази в базата
            _context.AccessRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Заявката е изпратена!";
            return RedirectToAction("Index");
        }

        // Админ одобрява заявка
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _context.AccessRequests.FindAsync(id);

            if (request == null) return NotFound();

            // Смени статуса на Одобрена
            request.Status = "Approved";
            request.AdminResponse = "Одобрено";
            request.ApprovedByAdminId = _userManager.GetUserId(User);
            request.ApprovalDate = DateTime.Now;

            // Създай автоматично лог при одобрение
            var log = new AccessLog
            {
                UserId = request.UserId,
                RoomId = request.RoomId,
                EntryTime = DateTime.Now,
                Status = "Approved"
            };
            _context.AccessLogs.Add(log);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Заявката е одобрена!";
            return RedirectToAction("Index");
        }

        // Админ отказва заявка
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deny(int id)
        {
            var request = await _context.AccessRequests.FindAsync(id);

            if (request == null) return NotFound();

            // Смени статуса на Отказана
            request.Status = "Denied";
            request.AdminResponse = "Отказано";
            request.ApprovedByAdminId = _userManager.GetUserId(User);
            request.ApprovalDate = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Error"] = "Заявката е отказана.";
            return RedirectToAction("Index");
        }
    }
}
