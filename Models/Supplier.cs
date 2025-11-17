using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalSupplyChainManagementSystem.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string ContactPerson { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;

        // Navigation collections – must be initialized
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
