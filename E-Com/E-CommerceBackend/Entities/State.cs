using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.Entities
{
    public class State
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateId { get; set; }
        public string statename { get; set; }

        // Foreign Key to Country
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
