using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IEmailService
    {
        Task sendEmailAsync(string email, string subject, string body);
    }
}
