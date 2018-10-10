using System;
using System.Collections.Generic;
using System.Text;

namespace IRunesWebApp.Model
{
    public abstract class BaseEntity<TKeyIdentifier>
    {
        public TKeyIdentifier Id { get; set; }
    }
}
