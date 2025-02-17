using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.Entities
{
    public class CartMastertable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartMasterId { get; set; }
        [Required]
        public int UsertableId { get; set; }

    }

}
