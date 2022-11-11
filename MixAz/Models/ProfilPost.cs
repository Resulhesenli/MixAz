using System;
using System.Collections.Generic;

#nullable disable

namespace MixAz.Models
{
    public partial class ProfilPost
    {
        public ProfilPost()
        {
            Comments = new HashSet<Comment>();
            ProfilPhotos = new HashSet<ProfilPhoto>();
            ProfilPostLikes = new HashSet<ProfilPostLike>();
        }

        public int ProfilPostId { get; set; }
        public int? ProfilPostUserId { get; set; }
        public string ProfilPostDescription { get; set; }
        public DateTime ProfilPostWriteDate { get; set; }
        public string ProfilPostPhotoUrl { get; set; }

        public virtual User ProfilPostUser { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ProfilPhoto> ProfilPhotos { get; set; }
        public virtual ICollection<ProfilPostLike> ProfilPostLikes { get; set; }
    }
}
