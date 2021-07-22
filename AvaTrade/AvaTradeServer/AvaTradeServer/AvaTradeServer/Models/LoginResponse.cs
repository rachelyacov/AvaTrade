using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaTradeServer.Models
{
    public class LoginResponse
    {
        public bool isBlocked { get; set; }
        public bool isLoginSuccess { get; set; }
    }
}