﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaTradeServer.Models
{
    public class LoginRequest
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
}