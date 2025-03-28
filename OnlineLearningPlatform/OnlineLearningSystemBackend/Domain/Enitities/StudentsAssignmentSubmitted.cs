using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enitities
{
    public class StudentsAssignmentSubmitted
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubmittedId { get; set; }
        public int studentId { get; set; }
        public int AssignmentId { get; set; }
    }
}
