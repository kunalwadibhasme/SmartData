using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model.Course
{
    public class UpdateCourseDto
    {
        public required int CourseId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public required int InstructorId { get; set; }
        public required string StartDate { get; set; }
        public required string EndDate { get; set; }
    }
}
