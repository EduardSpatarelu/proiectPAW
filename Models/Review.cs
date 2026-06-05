using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BARmonica.Web.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ClientName { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; } // Note de la 1 la 5 stele

        public string? Comment { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.Now;

        // Legatura cu Produsul recenzat
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}