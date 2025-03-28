using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model.Assignment
{
    public class AssignmentGetDto
    {
        public int AssignmentId { get; set; }
        public string AssignmentName { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }

    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<OptionDto> Options { get; set; }
        public AnswerDto Answer { get; set; }
    }

    public class OptionDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
    }

    public class AnswerDto
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
    }

}
