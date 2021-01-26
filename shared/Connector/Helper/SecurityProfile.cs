using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connector.Model
{
    public class SecurityProfile
    {
        public string Secret { get; set; }

        public DateTime Validity { get; set; }


        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
