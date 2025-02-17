using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.Entities
{
    public class Usertype
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int typeId { get; set; }
        [Required]
        [MaxLength(250)]
        public string usertype { get; set; }
    }
}
