using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makeup.Entities
{
    public class ApiUser
    {
        public int ApiUserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public string? Role { get; set; }
    }
}
