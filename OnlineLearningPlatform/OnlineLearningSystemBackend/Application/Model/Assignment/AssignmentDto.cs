using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model.Assignment
{
    public class AssignmentDto
    {
        public required int courseId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string testLink { get; set; }
        public required string DueDate { get; set; }
    }
}
