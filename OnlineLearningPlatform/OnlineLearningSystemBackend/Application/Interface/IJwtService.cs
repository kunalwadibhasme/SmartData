using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IJwtService
    {
        string GenerateToken(int UserId, string Email, string roleType, string ApiKey);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    }
}
