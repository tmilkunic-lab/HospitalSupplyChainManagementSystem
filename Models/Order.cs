using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalSupplyChainManagementSystem.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Foreign key + navigation property
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Required]
        public int QuantityOrdered { get; set; }

        // Foreign key + navigation property
        [Required]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        [StringLength(50)]
        public string Status { get; set; } = "Pending";
    }
}
