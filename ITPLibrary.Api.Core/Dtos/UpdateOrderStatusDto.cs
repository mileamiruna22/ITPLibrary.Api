﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Core.Dtos
{
    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }
        public string NewStatus { get; set; }
    }
}
