using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceBackend.Entities
{
    public class Usertable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsertableId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required, EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        public int UsertypeId { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [StringLength (10)]
        public string Mobile { get; set; }
        [Required]
        [StringLength (40)]
        public string Address { get; set; }
        [Required]
        [StringLength(6)]
        public string ZipCode { get; set; }
        [Required]
        public string Profileimage {  get; set; }
        [Required]
        public string StateId { get; set; }
        [Required]
        public string CountryId{ get; set; }


    }
}
