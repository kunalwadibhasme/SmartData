using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Enitities
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }
        public required string Title {  get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public required int InstructorId { get; set; }
        public required string StartDate { get; set; }
        public required string EndDate { get; set; }


        public ICollection<Assignment> Assignments { get; set; } // Navigation for one-to-many

    }
}
