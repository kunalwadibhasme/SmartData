using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.DTOs
{
    public class getPatientbyProviderIdDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string Status { get; set; }
        public float fees { get; set; }
        public string? ChiefComplaint { get; set; }
    }
}
