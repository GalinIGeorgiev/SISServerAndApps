using System;
using System.Collections.Generic;
using System.Text;

namespace IRunesWebApp.Model
{
    public class User : BaseEntity<string>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
    }
}
