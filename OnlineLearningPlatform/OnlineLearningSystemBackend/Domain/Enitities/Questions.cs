using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Enitities
{
    public class Questions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required]
        [StringLength(500)]
        public string QuestionText { get; set; }

        [Required]
        [StringLength(50)]
        public string QuestionType { get; set; } // 'mcq' or 'descriptive'

        // Foreign key for the Test
        public int AssignmentId { get; set; }
        public Assignment Assignments { get; set; }

        // One-to-many relationship with Option (only for MCQ)
        public ICollection<Options> Options { get; set; }

        // One-to-one relationship with Answer
        public Answer Answer { get; set; }
    }
}
