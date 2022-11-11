using System;
using System.Collections.Generic;

#nullable disable

namespace MixAz.Models
{
    public partial class Sherh
    {
        public int SherhId { get; set; }
        public int? SherhUserId { get; set; }
        public int? SherhPostId { get; set; }
        public string SherhDescription { get; set; }
        public DateTime SherhWriteDate { get; set; }

        public virtual Post SherhPost { get; set; }
        public virtual User SherhUser { get; set; }
    }
}
