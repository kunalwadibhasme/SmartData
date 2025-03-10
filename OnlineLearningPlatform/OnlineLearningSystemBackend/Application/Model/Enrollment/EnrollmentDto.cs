using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model.Enrollment
{
    public class EnrollmentDto
    {
        public required int StudentId { get; set; }
        public required int CourseId { get; set; }
    }
}
