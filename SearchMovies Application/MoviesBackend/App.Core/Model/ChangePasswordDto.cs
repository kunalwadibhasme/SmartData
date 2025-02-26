using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public class ChangePasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
