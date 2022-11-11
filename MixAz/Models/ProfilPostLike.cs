using System;
using System.Collections.Generic;

#nullable disable

namespace MixAz.Models
{
    public partial class ProfilPostLike
    {
        public int ProfilPostLikeId { get; set; }
        public int? ProfilPostLikeUserId { get; set; }
        public int? ProfilPostLikePostId { get; set; }
        public bool? ProfilPostIsLike { get; set; }

        public virtual ProfilPost ProfilPostLikePost { get; set; }
        public virtual User ProfilPostLikeUser { get; set; }
    }
}
