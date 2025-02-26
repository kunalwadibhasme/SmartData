using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public int UsertableId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_]{10}$")]
        public string ProductCode { get; set; }
        public string? ProductImagePath { get; set; } // Path to the uploaded image
        [Required]
        [StringLength(50)]
        public string Category { get; set; }
        [Required]
        [StringLength(50)]
        public string Brand { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public float SellingPrice { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public float PurchasePrice { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string PurchaseDate { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        [Required]
        public bool isDeleted { get; set; } = false;
    }
}
