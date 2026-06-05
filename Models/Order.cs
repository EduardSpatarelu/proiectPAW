using System.ComponentModel.DataAnnotations;

namespace BARmonica.Web.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string ShippingAddress { get; set; } = string.Empty;

        public string Status { get; set; } = "În procesare";

        public decimal TotalAmount { get; set; }

        // O comanda contine mai multe produse cumparate
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}