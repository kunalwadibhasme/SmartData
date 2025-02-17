using System.Data;

namespace E_CommerceBackend.Interfaces
{
    public interface IDapperDbConnection
    {
        public IDbConnection CreateConnection();
    }
}
