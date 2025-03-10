using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enitities
{
    public enum UserRole
    {
        Admin,        // Value 0
        Instructor,   // Value 1
        Student       // Value 2
    }
}
