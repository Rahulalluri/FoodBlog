﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenGeneration.Model
{
    public class SecurityProfile
    {
        public string Secret { get; set; }

        public DateTime Validity { get; set; }
    }
}
