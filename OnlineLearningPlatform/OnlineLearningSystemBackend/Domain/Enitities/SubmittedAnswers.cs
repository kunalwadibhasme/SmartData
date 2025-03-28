using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enitities
{
    public class SubmittedAnswers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool Correct { get; set; }
    }
}
