using System;
using System.Collections.Generic;

#nullable disable

namespace MixAz.Models
{
    public partial class ProfilPhoto
    {
        public int ProfilPhotoId { get; set; }
        public string ProfilPhotoUrl { get; set; }
        public int? ProfilPhotoPostId { get; set; }

        public virtual ProfilPost ProfilPhotoPost { get; set; }
    }
}
