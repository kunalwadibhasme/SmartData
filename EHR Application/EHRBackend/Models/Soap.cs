using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.Models
{
    public class Soap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int AppointmentId { get; set; }
        [Required]
        public string Subjective { get; set; }
        [Required]
        public string Objective { get; set; }
        [Required]
        public string Assessment { get; set; }
        [Required]
        public string Planning { get; set; }
    }
}
