﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Messaging.ProductDetailUpdated
{
    public record ProductDetailUpdatedCommand(ProductService.Messages.ProductDetailUpdated ProductDetailUpdated);
}
