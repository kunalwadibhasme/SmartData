using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.DTOs
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public int UsertypeId { get; set; }
        public string DateOfBirth { get; set; }
     
        public string Mobile { get; set; }
        
        public string Address { get; set; }
        
        public string ZipCode { get; set; }
        
        public IFormFile Profileimage { get; set; }
        
        public string StateId { get; set; }
        
        public string CountryId { get; set; }

    }
}
