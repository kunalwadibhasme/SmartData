using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enitities
{
    public class Enrollments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int EnrollmentId { get; set; }
        public required int StudentId { get; set; }
        public required int CourseId { get; set; }
        public required Status status { get; set; }
    }
}
public enum Status
{
    Active,
    Completed
}