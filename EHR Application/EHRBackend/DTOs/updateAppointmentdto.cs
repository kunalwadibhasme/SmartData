using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.DTOs
{
    public class updateAppointmentdto
    {
        public int AppointmentId { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string? ChiefComplaint { get; set; }

    }
}
