using System;
using System.Collections.Generic;
using System.Text;

namespace Connector.Model
{
    public class UserRole
    {
        public string RoleName { get; set; }

        public IDictionary<string, IEnumerable<String>> Operations { get; set; }

        public UserRole(string name, IEnumerable<string> operations)
        {
            RoleName = name;
            Operations = new Dictionary<string, IEnumerable<string>>() { { name, operations } };
        }
    }
}
