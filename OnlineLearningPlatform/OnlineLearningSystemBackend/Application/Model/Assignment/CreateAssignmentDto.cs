using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model.Assignment
{
    public class CreateAssignmentDto
    {
        public string TestTitle { get; set; }    
        public string Description { get; set; }  
        public string CourseId { get; set; }     
        public string DueDate { get; set; }    
        public List<QuestionDTO> Questions { get; set; } 
    }
    

    public class QuestionDTO
    {
        public string QuestionType { get; set; }  
        public string QuestionText { get; set; }  
        public List<string> Options { get; set; } 
        public string Answer { get; set; }        
    }

    

}
