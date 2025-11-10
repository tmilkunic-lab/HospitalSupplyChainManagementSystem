using System.ComponentModel.DataAnnotations;

namespace HospitalSupplyChainManagementSystem.Models
{
    public class Vendor
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ContactName { get; set; }

        [Phone, Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Url, Display(Name = "Website")]
        public string? Website { get; set; }

        [StringLength(50)]
        public string? Status { get; set; } // e.g., Active, On Hold, Do Not Order
    }
}
