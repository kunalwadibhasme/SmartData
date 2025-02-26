using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.Models
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }
        [Required]
        [StringLength(10)]
        public string shortname { get; set; }
        [Required]
        [StringLength(100)]
        public string name { get; set; }
        [Required]
        public int phonecode { get; set; }

        // Navigation property - A country can have multiple states
        public ICollection<State> States { get; set; }
    }
}
