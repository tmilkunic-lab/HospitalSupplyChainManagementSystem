using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSupplyChainManagementSystem.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Foreign key
        public int SupplierId { get; set; }

        // Navigation property
        public Supplier Supplier { get; set; } = null!;
    }
}

