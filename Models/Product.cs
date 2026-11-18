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

        // NOU: Cerinta Comert Electronic - Specificatii
        [Required(ErrorMessage = "Specificațiile sunt obligatorii!")]
        [StringLength(500, ErrorMessage = "Specificațiile nu pot depăși 500 de caractere.")]
        public string Specifications { get; set; } = "Volum: 0.7L | Alcool: 40% | Origine: România";

        [Required(ErrorMessage = "Prețul este obligatoriu!")]
        [Range(1, 10000, ErrorMessage = "Prețul trebuie să fie între 1 și 10.000 Lei.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Cantitatea din stoc este obligatorie!")]
        [Range(0, 5000, ErrorMessage = "Stocul nu poate fi mai mic de 0.")]
        public int StockQuantity { get; set; }

        // NOU: Cerinta Comert Electronic - Modalitate de livrare
        [Required(ErrorMessage = "Modalitatea de livrare este obligatorie!")]
        [StringLength(150, ErrorMessage = "Modalitatea de livrare nu poate depăși 150 de caractere.")]
        public string DeliveryMethod { get; set; } = "Curier rapid (24-48h, ambalaj antisoc)";

        public string? ImageUrl { get; set; }

        // Relatiile
        [Required(ErrorMessage = "Te rugăm să selectezi o categorie!")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Te rugăm să selectezi un furnizor/producător!")]
        public int ProducerId { get; set; }
        public Producer? Producer { get; set; }
    }
}