﻿namespace E_CommerceBackend.DTOs
{
    public class CompletedAppointmentDetailsOfProviderDto
    {
        public int AppointmentId { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Specialization { get; set; }
        public string Profileimage { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Planning { get; set; }
    }
}
