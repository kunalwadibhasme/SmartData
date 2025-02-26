using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.DTOs
{
    public class AppointmentDto
    {
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }

        public string Status { get; set; }
        public float fees { get; set; }
        public string? ChiefComplaint { get; set; }
    }
}
