using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceBackend.Entities
{
    public class SalesMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string InvoiceId { get; set; }
        [Required]
        public int UsertableId { get; set; }
        [Required]
        public string InvoiceDate { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public float TotalPrice { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public string DeliveryZipcode { get; set; }
        [Required]
        public string DeliveryStateId { get; set; }
        [Required]
        public string DeliveryCountryId { get; set; }
    }
}
