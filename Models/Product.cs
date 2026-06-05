using System.ComponentModel.DataAnnotations;

namespace BARmonica.Web.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele produsului este obligatoriu!")]
        [StringLength(100, ErrorMessage = "Numele nu poate depăși 100 de caractere.")]
        public string Name { get; set; }

        [StringLength(5000, ErrorMessage = "Descrierea nu poate depăși 5000 de caractere.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Prețul este obligatoriu!")]
        [Range(1, 10000, ErrorMessage = "Prețul trebuie să fie între 1 și 10.000 Lei.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Cantitatea din stoc este obligatorie!")]
        [Range(0, 5000, ErrorMessage = "Stocul nu poate fi mai mic de 0.")]
        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        // Relatiile
        [Required(ErrorMessage = "Te rugăm să selectezi o categorie!")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Te rugăm să selectezi un producător!")]
        public int ProducerId { get; set; }
        public Producer? Producer { get; set; }
    }
}