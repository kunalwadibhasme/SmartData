using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.DTOs
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public int UsertypeId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public int GenderId { get; set; }
        public int BloodGroupId { get; set; }
        //for provider only
        public string? Qualification { get; set; }
        public int? SpecializationId { get; set; }
        public string? RegisterationNo { get; set; }
        public float? VisitingCharge { get; set; }
    }
}
