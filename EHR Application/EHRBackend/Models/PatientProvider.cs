using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.Models
{
    public class PatientProvider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [StringLength(10)]
        public string Mobile { get; set; }
        [Required]
        [StringLength(40)]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }    

        [Required]
        [StringLength(6)]
        public string PinCode { get; set; }
        [Required]
        public string Profileimage { get; set; }
        [Required]
        public int StateId { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public int BloodGroupId { get; set; }

        //for provider only
        public string? Qualification { get; set; }
        public int? SpecializationId { get; set; }
        public string? RegisterationNo { get; set; }
        public float? VisitingCharge { get; set; }
    }
}
