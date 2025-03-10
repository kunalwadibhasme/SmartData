using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enitities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public UserRole Role { get; set; }  // The role will be mapped as an enum
        public string ApiKey { get; set; }
        public DateTime CreatedAt { get; set; }  // Date of user creation
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

    }

}
