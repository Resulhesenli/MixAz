using System;
using System.Collections.Generic;

#nullable disable

namespace MixAz.Models
{
    public partial class Post
    {
        public Post()
        {
            Likes = new HashSet<Like>();
            Photos = new HashSet<Photo>();
            Sherhs = new HashSet<Sherh>();
        }

        public int PostId { get; set; }
        public int? PostUserId { get; set; }
        public string PostDescription { get; set; }
        public DateTime PostWriteDate { get; set; }
        public string PostName { get; set; }
        public int? PostCategoryId { get; set; }
        public bool? PostShare { get; set; }
        public int? PostViewCount { get; set; }

        public virtual Category PostCategory { get; set; }
        public virtual User PostUser { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Sherh> Sherhs { get; set; }
    }
}
