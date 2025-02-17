using E_CommerceBackend.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace E_CommerceBackend.Services
{
    public class DapperDbConnection : IDapperDbConnection
    {
        public readonly string _connectionString;

        public DapperDbConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
