using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class PasswordResetRequest
    {
        public string email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
