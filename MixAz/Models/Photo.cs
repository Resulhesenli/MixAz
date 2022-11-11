using System;
using System.Collections.Generic;

#nullable disable

namespace MixAz.Models
{
    public partial class Photo
    {
        public int PhotoId { get; set; }
        public string PhotoUrl { get; set; }
        public int? PhotoPostId { get; set; }

        public virtual Post PhotoPost { get; set; }
    }
}
