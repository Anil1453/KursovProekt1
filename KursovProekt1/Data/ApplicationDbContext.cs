using ControlPanel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ControlPanel.Data
{
    // Базата данни - тук дефинираме таблиците
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Таблици
        public DbSet<Room> Rooms { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }
        public DbSet<AccessRequest> AccessRequests { get; set; }
    }
}