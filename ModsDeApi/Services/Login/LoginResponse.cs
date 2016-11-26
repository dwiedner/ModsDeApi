using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModsDeApi.Services.Login
{
    internal class LoginResponse
    {
        public int UserId { get; set; }
        public string LoginId { get; set; }
        public int Lifetime { get; set; }
    }
}
