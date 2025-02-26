using E_CommerceBackend.DTOs;
using E_CommerceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace E_CommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;
        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> ProviderbasedonSpeciality(int Id)
        {
            var type = await _appointmentService.ProviderbasedonSpeciality(Id);
            return Ok(type);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> SaveAppointment(AppointmentDto appointmentDto)
        {
            bool result = await _appointmentService.SaveAppointment(appointmentDto);
            return Ok(result);
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> CancelAppointment(int Id)
        {
            bool result = await _appointmentService.CancelAppointment(Id);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateAppointment(updateAppointmentdto updateAppointmentdto)
        {
            bool result = await _appointmentService.UpdateAppointment(updateAppointmentdto);
            return Ok(result);
        }

        

        [HttpGet("[action]")]
        public async Task<ActionResult> AllAppointments()
        {
            var result = await _appointmentService.AllAppointments();
            return Ok(result);
        }
        

        [HttpGet("[action]")]
        public async Task<ActionResult> AppointmentsByProviderId(int Id)
        {
            var result = await _appointmentService.AppointmentsByProviderId(Id);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> AppointmentsByPatientId(int Id)
        {
            var result = await _appointmentService.AppointmentsByPatientId(Id);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> ListOfPatient()
        {
            var result = await _appointmentService.ListOfPatient();
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetpatientbyProviderId(int Id)
        {
            var result = await _appointmentService.GetPatientByProviderId(Id);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GoToAppointmentDetails(int AppointmentId)
        {
            var patient = await _appointmentService.GoToAppointmentDetails(AppointmentId);
            return Ok(patient);
        }
        [HttpPost("[action]")]
        public async Task<ActionResult> SaveSoap(AddSoapdto addSoapdto)
        {
            var result = await _appointmentService.SaveSoap(addSoapdto);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> CompletedAppointmentDetailsforProvider( int Id)
        {
            var result = await _appointmentService.CompletedAppointmentDetailsforProvider(Id);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> CompletedAppointmentDetailsforPatient(int Id)
        {
            var result = await _appointmentService.CompletedAppointmentDetailsforPatient(Id);
            return Ok(result);
        }

    }
}
