using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enitities
{
    public class Options
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionId { get; set; }

        [Required]
        [StringLength(255)]
        public string OptionText { get; set; }

        // Foreign key for the Question
        public int QuestionId { get; set; }
        public Questions Question { get; set; }
    }
}
