using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IRunes.Models
{
    public abstract class BaseEntity<TKeyIdentifier>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKeyIdentifier Id { get; set; }
    }
}
