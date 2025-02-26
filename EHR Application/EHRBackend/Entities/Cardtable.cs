using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceBackend.Entities
{
    public class Cardtable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CardtableId { get; set; }
        [Required]
        [StringLength(16)]
        public long CardNumber { get; set; }
        public string ExpirtDate { get; set; }
        [Required]
        [StringLength(3)]
        public int CVV { get; set; }
    }
}




