using System.ComponentModel.DataAnnotations;

namespace BARmonica.Web.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele categoriei este obligatoriu!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Numele trebuie să aibă între 3 și 50 de caractere.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Descrierea nu poate depăși 500 de caractere.")]
        public string? Description { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}