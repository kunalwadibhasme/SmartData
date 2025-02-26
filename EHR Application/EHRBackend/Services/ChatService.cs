using Dapper;
using E_CommerceBackend.Database;
using E_CommerceBackend.DTOs;
using E_CommerceBackend.Interfaces;
using E_CommerceBackend.Models;
using System.Data;

namespace E_CommerceBackend.Services
{
    public class ChatService
    {
        private readonly EhrDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IDapperDbConnection _DapperdbConnection;
        private readonly IWebHostEnvironment _env;

        public ChatService(EhrDbContext ehrDbContext, IConfiguration configuration, IDapperDbConnection dapperDbConnection, IWebHostEnvironment env)
        {
            _appDbContext = ehrDbContext;
            _configuration = configuration;
            _env = env;
            _DapperdbConnection = dapperDbConnection;
        }

        public async Task<List<ChatDto>> GetPatientbyProviderId(int providerid)
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = @"
                SELECT 
                pp.Id As PatientId, 
                pp.FirstName, 
                pp.LastName
                FROM Appointment AS a
                JOIN PatientProvider AS pp ON pp.Id = a.PatientId
                WHERE a.ProviderId = @ProviderId AND a.Status = 'Scheduled'";
                var parameters = new { ProviderId = providerid };

                var result = await db.QueryAsync<ChatDto>(sql, parameters);
                return result.ToList();
            }
        }

        public async Task<List<ChatDto>> GetProviderByPatientId(int patientid)
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = @"
                SELECT DISTINCT
                pp.Id As PatientId, 
                pp.FirstName, 
                pp.LastName
                FROM Appointment AS a
                JOIN PatientProvider AS pp ON pp.Id = a.ProviderId
            WHERE a.PatientId = @PatientId AND a.Status = 'Scheduled'";
                var parameters = new { PatientId = patientid };

                var result = await db.QueryAsync<ChatDto>(sql, parameters);
                return result.ToList();
            }
        }
    }
}
