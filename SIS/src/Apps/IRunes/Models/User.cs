using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Models
{
    public class User : BaseEntity<Guid>
    {
        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public string Email { get; set; }
    }
}
