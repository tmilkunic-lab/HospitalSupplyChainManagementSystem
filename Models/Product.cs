using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSupplyChainManagementSystem.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
