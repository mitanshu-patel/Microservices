﻿using OrderService.Domain.Orders.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders.Get
{
    public record GetOrderDetailsResult(OrderDetailsDto OrderDetails);
}
