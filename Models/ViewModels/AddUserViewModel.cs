using System.ComponentModel.DataAnnotations;

namespace RentACar_ip.Models.ViewModels
{
    public class AddUserViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // "admin" veya "employee"
    }
}
