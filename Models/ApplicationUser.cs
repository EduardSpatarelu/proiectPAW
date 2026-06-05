using Microsoft.AspNetCore.Identity;

namespace BARmonica.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfilePicturePath { get; set; }

        // Câmpurile noi pentru magazinul real:
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}