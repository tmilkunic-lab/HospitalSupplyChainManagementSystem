using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalSupplyChainManagementSystem.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string ContactPerson { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
