using System.ComponentModel.DataAnnotations;

namespace BARmonica.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu!")]
        [EmailAddress(ErrorMessage = "Format de email invalid!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Prenumele este obligatoriu!")]
        [Display(Name = "Prenume")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Numele de familie este obligatoriu!")]
        [Display(Name = "Nume")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Numărul de telefon este obligatoriu!")]
        [Phone(ErrorMessage = "Format de telefon invalid!")]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Data nașterii este obligatorie!")]
        [DataType(DataType.Date)]
        [Display(Name = "Data Nașterii")]
        [MinimumAge(18)] // RESTRICTIA DE VARSTA
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email-ul este obligatoriu!")]
        [EmailAddress(ErrorMessage = "Format de email invalid!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmarea parolei este obligatorie!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parolele nu se potrivesc!")]
        public string ConfirmPassword { get; set; }

        // Aici este cheia pentru punctul de pe barem cu stocarea imaginii:
        public IFormFile? ProfilePicture { get; set; }
    }
}