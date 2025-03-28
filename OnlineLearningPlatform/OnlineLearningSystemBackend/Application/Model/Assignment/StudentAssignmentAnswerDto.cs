using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model.Assignment
{
    public class StudentAssignmentAnswerDto
    {
        public int StudentId { get; set; }  
        public int AssignmentId { get; set; }  
        public AssignmentAnswerDto AssignmentAnswerDto { get; set; }
    }

    public class AssignmentAnswerDto
    {
        // Initialize the dictionary to avoid null references
        public Dictionary<int, string> Answers { get; set; } = new Dictionary<int, string>();
    }
}
