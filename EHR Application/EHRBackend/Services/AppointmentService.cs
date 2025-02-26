using Dapper;
using E_CommerceBackend.Database;
using E_CommerceBackend.DTOs;
using E_CommerceBackend.Interfaces;
using E_CommerceBackend.Models;
using E_CommerceBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Data;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Components.Web;
using Mapster;
using Microsoft.Identity.Client;
using E_CommerceBackend.Migrations;

namespace E_CommerceBackend.Services
{
    public class AppointmentService
    {
        private readonly EhrDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IDapperDbConnection _DapperdbConnection;
        private readonly IWebHostEnvironment _env;

        public AppointmentService(EhrDbContext ehrDbContext, IConfiguration configuration, IDapperDbConnection dapperDbConnection, IWebHostEnvironment env)
        {
            _appDbContext = ehrDbContext;
            _configuration = configuration;
            _env = env;
            _DapperdbConnection = dapperDbConnection;
        }

        public async Task<List<PatientProvider>> ProviderbasedonSpeciality(int id)
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM PatientProvider WHERE SpecializationId = @SpecializationId";
                var result = await db.QueryAsync<PatientProvider>(sql, new { SpecializationId = id});
                return result.ToList();
            }
        }

        public async Task<Boolean> SaveAppointment(AppointmentDto appointmentDto)
        {
            var appointment = new Appointment()
            {
                PatientId = appointmentDto.PatientId,
                ProviderId = appointmentDto.ProviderId,
                AppointmentDate = appointmentDto.AppointmentDate,
                AppointmentTime = appointmentDto.AppointmentTime,
                Status = appointmentDto.Status,
                fees = appointmentDto.fees,
                ChiefComplaint = appointmentDto.ChiefComplaint
            };
            await _appDbContext.Appointment.AddAsync(appointment);
            await _appDbContext.SaveChangesAsync();
            var patientmail = await _appDbContext.PatientProvider.Where(p => p.Id == appointmentDto.PatientId).FirstOrDefaultAsync();
            var providermail = await _appDbContext.PatientProvider.Where(p => p.Id == appointmentDto.ProviderId).FirstOrDefaultAsync();
            //Mail to Patient

            var emailBody = $"{patientmail.FirstName} {patientmail.LastName}, Your Appointment Has Been Scheduled with {providermail.FirstName} {providermail.LastName} " +
                                $"on {appointmentDto.AppointmentDate}, at {appointmentDto.AppointmentTime}.";
            await sendEmailAsync(patientmail.Email, "Appointment Scheduled", emailBody);
            //Mail to Provider
            var emailBody1 = $" {providermail.FirstName} {providermail.LastName}, Your Appointment Has Been Scheduled with {patientmail.FirstName} {patientmail.LastName} " +
                                $"on {appointmentDto.AppointmentDate}, at {appointmentDto.AppointmentTime}.";
            await sendEmailAsync(providermail.Email, "Appointment Scheduled", emailBody1);

            return true;
        }

        public async Task<List<Appointment>> AllAppointments()
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Appointment";
                var result = await db.QueryAsync<Appointment>(sql);
                return result.ToList();
            }
        }

        public async Task<bool> UpdateAppointment(updateAppointmentdto updateAppointmentdto)
        {
            var appointment = await _appDbContext.Appointment.Where(a => a.AppointmentId == updateAppointmentdto.AppointmentId).FirstOrDefaultAsync();
            if (appointment!=null)
            {
                appointment.AppointmentDate = updateAppointmentdto.AppointmentDate;
                appointment.AppointmentTime = updateAppointmentdto.AppointmentTime;
                appointment.ChiefComplaint = updateAppointmentdto.ChiefComplaint;
                _appDbContext.Appointment.Update(appointment);
                await _appDbContext.SaveChangesAsync();

                var patientmail = await _appDbContext.PatientProvider.Where(p => p.Id == appointment.PatientId).FirstOrDefaultAsync();
                var providermail = await _appDbContext.PatientProvider.Where(p => p.Id == appointment.ProviderId).FirstOrDefaultAsync();
                //Mail to Patient

                var emailBody = $"{patientmail.FirstName} {patientmail.LastName}, Your Appointment with {providermail.FirstName} {providermail.LastName} Has Been Updated. It is Scheduled" +
                                    $"on {appointment.AppointmentDate}, at {appointment.AppointmentTime}.";
                await sendEmailAsync(patientmail.Email, "Appointment Scheduled", emailBody);
                //Mail to Provider
                var emailBody1 = $" {providermail.FirstName} {providermail.LastName}, Your Appointment with {patientmail.FirstName} {patientmail.LastName} Has Been Updated. It is Scheduled" +
                                    $"on {appointment.AppointmentDate}, at {appointment.AppointmentTime}.";
                await sendEmailAsync(providermail.Email, "Appointment Scheduled", emailBody1);
                return true;
            }
            return false;
        }

        public async Task<List<Appointment>> AppointmentsByProviderId(int Id)
        {
            using(IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Appointment WHERE ProviderId = @ProviderId ";
                var result = await db.QueryAsync<Appointment>(sql, new { ProviderId = Id});
                return result.ToList();
            }
        }

        public async Task<bool> CancelAppointment(int Id)
        {
            var appointment = await _appDbContext.Appointment.Where(a => a.AppointmentId == Id).FirstOrDefaultAsync();
            appointment.Status = "Cancelled";
            _appDbContext.Appointment.Update(appointment);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<getPatientbyProviderIdDto>> AppointmentsByPatientId(int Id)
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = @"
                SELECT 
                pp.Id, 
                pp.FirstName, 
                pp.LastName,
                a.AppointmentId,
                a.AppointmentDate, 
                a.AppointmentTime, 
                a.Status, 
                a.Fees, 
                a.ChiefComplaint
                FROM Appointment AS a
                JOIN PatientProvider AS pp ON a.ProviderId = pp.Id
                WHERE a.PatientId = @PatientId AND a.Status = 'Scheduled' AND a.AppointmentDate>= GETDATE()
                ORDER BY a.AppointmentDate";

                var parameters = new { PatientId = Id };

                var result = await db.QueryAsync<getPatientbyProviderIdDto>(sql, parameters);
                return result.ToList();
            }
        }

        public async Task<List<PatientProvider>> ListOfPatient()
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM PatientProvider WHERE UsertypeId = 2";
                var result = await db.QueryAsync<PatientProvider>(sql);
                return result.ToList();
            }
        }

        public async Task<List<getPatientbyProviderIdDto>> GetPatientByProviderId(int Id)
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = @"
                SELECT 
                pp.Id, 
                pp.FirstName, 
                pp.LastName,
                a.AppointmentId,
                a.AppointmentDate, 
                a.AppointmentTime, 
                a.Status, 
                a.Fees, 
                a.ChiefComplaint
                FROM Appointment AS a
                JOIN PatientProvider AS pp ON a.PatientId = pp.Id
                WHERE a.ProviderId = @ProviderId AND a.Status = 'Scheduled' AND a.AppointmentDate>= GETDATE()
                ORDER BY a.AppointmentDate";
                var parameters = new { ProviderId = Id };
                var result = await db.QueryAsync<getPatientbyProviderIdDto>(sql, parameters);
                return result.ToList();
            }
        }

        public async Task<bool> SaveSoap(AddSoapdto addSoapdto)
        {
            if(addSoapdto!=null)
            {
                var addSoap = new Soap()
                {
                    AppointmentId = addSoapdto.AppointmentId,
                    Subjective = addSoapdto.Subjective,
                    Objective = addSoapdto.Objective,
                    Assessment = addSoapdto.Assessment,
                    Planning = addSoapdto.Planning
                };
                _appDbContext.Soap.Add(addSoap);
                await _appDbContext.SaveChangesAsync();
                var appointment = await _appDbContext.Appointment.Where(a => a.AppointmentId == addSoapdto.AppointmentId).FirstOrDefaultAsync();
                appointment.Status = "Completed";
                _appDbContext.Appointment.Update(appointment);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<GoToAppointmentDto> GoToAppointmentDetails(int Id)
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = @"
                SELECT 
                pp.FirstName, pp.LastName, pp.Profileimage,
                pp.DateOfBirth, DATEDIFF(year, pp.DateOfBirth, GETDATE()) AS Age, g.gender AS Gender,
                a.AppointmentId, a.AppointmentDate, a.AppointmentTime, a.ChiefComplaint
                FROM Appointment AS a
                INNER JOIN PatientProvider AS pp ON a.PatientId = pp.Id
                INNER JOIN Gender AS g ON pp.GenderId = g.Id
                WHERE a.AppointmentId = @AppointmentId";
                var parameters = new { AppointmentId = Id };
                var result = await db.QueryAsync<GoToAppointmentDto>(sql, parameters);
                return result.FirstOrDefault();
            }
        }
        /// <summary>
        /// /////////////////////////////
        
        public async Task<List<CompletedAppointmentDetailsOfPatientdto>> CompletedAppointmentDetailsforProvider(int providerId)
        {
            //using (IDbConnection db = _DapperdbConnection.CreateConnection())
            //{
            //    string sql = @"
            //    SELECT
            //    a.AppointmentId,pp.FirstName, pp.LastName, pp.Profileimage, pp.DateOfBirth, 
            //    DATEDIFF(year, pp.DateOfBirth, GETDATE()) AS Age, g.gender As Gender, a.AppointmentDate, 
            //    a.AppointmentTime, s.Subjective,s.Objective, s.Assessment, s.Planning 
            //    FROM Appointment AS a
            //    INNER JOIN PatientProvider AS pp
            //    ON pp.Id = a.PatientId
            //    INNER JOIN Soap AS s
            //    ON a.AppointmentId = s.AppointmentId
            //    INNER JOIN Gender AS g
            //    ON pp.GenderId = g.Id
            //    where a.ProviderId = @ProviderId AND a.Status = 'Completed'";

            //    var parameters = new { ProviderId = providerId };
            //    var result = await db.QueryAsync<CompletedAppointmentDetailsOfPatientdto>(sql, parameters);
            //    return result.ToList();
            //}
            try
            {
                using (IDbConnection db = _DapperdbConnection.CreateConnection())
                {
                    string sql = @"
                    SELECT
                        a.AppointmentId, pp.FirstName, pp.LastName, pp.Profileimage, pp.DateOfBirth, DATEDIFF(year, pp.DateOfBirth, GETDATE()) AS Age, 
                        g.gender AS Gender, a.AppointmentDate, a.AppointmentTime, s.Subjective, s.Objective, s.Assessment, s.Planning
                    FROM Appointment AS a
                    INNER JOIN PatientProvider AS pp
                        ON pp.Id = a.PatientId
                    INNER JOIN Soap AS s
                        ON a.AppointmentId = s.AppointmentId
                    INNER JOIN Gender AS g
                        ON pp.GenderId = g.Id
                    WHERE a.ProviderId = @ProviderId AND a.Status = 'Completed'";

                    var parameters = new { ProviderId = providerId };

                    // Log SQL and Parameters (for debugging)
                    Console.WriteLine("Executing SQL Query: " + sql);
                    Console.WriteLine("ProviderId: " + providerId);

                    var result = await db.QueryAsync<CompletedAppointmentDetailsOfPatientdto>(sql, parameters);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur during execution
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }

        }

        public async Task<List<CompletedAppointmentDetailsOfProviderDto>> CompletedAppointmentDetailsforPatient(int patientId)
        {
            try
            {
                using (IDbConnection db = _DapperdbConnection.CreateConnection())
                {
                    string sql = @"
                    SELECT
                        a.AppointmentId, pp.FirstName, pp.LastName, pp.Profileimage, 
                        sp.Speciality as Specialization, g.Gender, a.AppointmentDate, 
                        a.AppointmentTime, s.Subjective, s.Objective, s.Assessment, s.Planning 
                    FROM Appointment AS a
                    INNER JOIN PatientProvider AS pp
                        ON pp.Id = a.ProviderId
                    INNER JOIN Soap AS s
                        ON a.AppointmentId = s.AppointmentId
                    INNER JOIN Gender AS g
                        ON pp.GenderId = g.Id
                    INNER JOIN Specialization AS sp
                        ON pp.SpecializationId = sp.Id
                    WHERE a.PatientId = @PatientId AND a.Status = 'Completed'";

                    var parameters = new { PatientId = patientId };
                    var result = await db.QueryAsync<CompletedAppointmentDetailsOfProviderDto>(sql, parameters);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// ///////////////////////////////////////////////////

        public async Task sendEmailAsync(string email, string subject, string body)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string fromEmail = _configuration["Smtp:From"]; // Use an environment variable for security
            string fromPassword = _configuration["Smtp:Password"]; // Use an environment variable for security

            try
            {
                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true; // Enable TLS encryption
                    client.Credentials = new NetworkCredential(fromEmail, fromPassword);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false, // Set to true if you want to send HTML emails
                    };

                    mailMessage.To.Add(email);

                    // Send email asynchronously
                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                // Log the exception details for debugging
                throw;
            }
        }


    }
}








