using ControlPanel.Data;
using ControlPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlPanel.Controllers
{
    [Route("api/rfid")]
    [ApiController]
    public class RfidController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RfidController(ApplicationDbContext context)
        {
            _context = context;
        }

        // RFID устройството изпраща имейл и стая
        // Системата връща Approved или Denied
        [HttpGet("check")]
        public async Task<IActionResult> Check(string userEmail, int roomId)
        {
            // Намери потребителя по имейл
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                return Ok(new { status = "Denied" });

            // Провери дали има одобрена заявка
            var hasAccess = await _context.AccessRequests
                .AnyAsync(r => r.UserId == user.Id
                            && r.RoomId == roomId
                            && r.Status == "Approved");

            // Запази лог
            _context.AccessLogs.Add(new AccessLog
            {
                UserId = user.Id,
                RoomId = roomId,
                EntryTime = DateTime.Now,
                Status = hasAccess ? "Approved" : "Denied"
            });
            await _context.SaveChangesAsync();

            return Ok(new { status = hasAccess ? "Approved" : "Denied" });
        }
    }
}