﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Data.Entities
{
    public class RemoteUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string MobileNo { get; set; }
    }
}
