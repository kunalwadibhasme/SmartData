using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceBackend.Entities
{
    public class SalesDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string InvoiceId { get; set;}
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public float SellingPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
