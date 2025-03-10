using Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class RegisterDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public UserRole Role { get; set; }
    }
}
