﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Common
{
    public class JWTResult
    {
        public bool IsValid
        {
            get; set;
        }
        public string Username
        {
            get; set;
        }
    }
}
