using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Models
{
    public class Track : BaseEntity<Guid>
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }

        public Guid AlbumId { get; set; }

        public virtual Album Album { get; set; }
    }
}
