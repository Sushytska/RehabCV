using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message, string emailOfCenter, string passwordOfEmail);
    }
}
