using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ControlPanel.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string? FullName { get; set; }

        [StringLength(50)]
        public string? Department { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Profil resmi dosya adı (wwwroot/uploads/ klasöründe saklanır)
        [StringLength(200)]
        public string? ProfilePicture { get; set; }

        public ICollection<AccessLog>? AccessLogs { get; set; }
        public ICollection<AccessRequest>? AccessRequests { get; set; }
    }
}
