using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enitities
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentId { get; set; }
        [ForeignKey("Course")]
        public int courseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string testLink { get; set; }
        public string DueDate { get; set; }

        public Course Course { get; set; } // Navigation property
                                           // One-to-many relationship with Question
        public ICollection<Questions> Questions { get; set; }
    }
}
