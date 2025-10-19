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

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int QuantityOrdered { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending";
    }
}
