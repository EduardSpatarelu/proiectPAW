using System.ComponentModel.DataAnnotations;

namespace BARmonica.Web.Models
{
    public class Producer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele producătorului este obligatoriu!")]
        [StringLength(100, ErrorMessage = "Numele nu poate depăși 100 de caractere.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Țara de origine este obligatorie!")]
        [StringLength(50, ErrorMessage = "Numele țării nu poate depăși 50 de caractere.")]
        public string CountryOfOrigin { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}