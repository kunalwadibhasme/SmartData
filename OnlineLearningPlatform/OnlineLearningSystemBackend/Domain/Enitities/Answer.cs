using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enitities
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }

        [Required]
        [StringLength(255)]
        public string AnswerText { get; set; }

        // Foreign key for the Question
        public int QuestionId { get; set; }
        public Questions Question { get; set; }
    }
}
