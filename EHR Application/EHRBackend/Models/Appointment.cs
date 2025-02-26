using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        [Required]
        public int ProviderId { get; set; }
        [Required]
        public string AppointmentDate { get; set; }
        [Required]
        public string AppointmentTime { get; set; }
        [Required]
        public string Status { get; set; }
        public float fees { get; set; }

        public string? ChiefComplaint { get; set; }

        
    }
}
