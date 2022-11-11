using System;
using System.Collections.Generic;

#nullable disable

namespace MixAz.Models
{
    public partial class Sher
    {
        public int SherId { get; set; }
        public int? SherUserId { get; set; }
        public int? SherPostId { get; set; }
        public string SherDescription { get; set; }
        public DateTime SherWriteDate { get; set; }

        public virtual Post SherPost { get; set; }
        public virtual User SherUser { get; set; }
    }
}
