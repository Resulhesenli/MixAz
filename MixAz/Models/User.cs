using System;
using System.Collections.Generic;

#nullable disable

namespace MixAz.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            FollowerFollowerFromUsers = new HashSet<Follower>();
            FollowerFollowerToUsers = new HashSet<Follower>();
            Likes = new HashSet<Like>();
            Posts = new HashSet<Post>();
            ProfilPostLikes = new HashSet<ProfilPostLike>();
            ProfilPosts = new HashSet<ProfilPost>();
            Sherhs = new HashSet<Sherh>();
        }

        public int UserId { get; set; }
        public int? UserGenderId { get; set; }
        public string UserNickName { get; set; }
        public string UserName { get; set; }
        public string UserMail { get; set; }
        public string UserSurname { get; set; }
        public string UserProfilePhotoUrl { get; set; }
        public string UserPassword { get; set; }
        public int? UserStatusId { get; set; }
        public bool? UserPrivate { get; set; }

        public virtual Gender UserGender { get; set; }
        public virtual Status UserStatus { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Follower> FollowerFollowerFromUsers { get; set; }
        public virtual ICollection<Follower> FollowerFollowerToUsers { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<ProfilPostLike> ProfilPostLikes { get; set; }
        public virtual ICollection<ProfilPost> ProfilPosts { get; set; }
        public virtual ICollection<Sherh> Sherhs { get; set; }
    }
}
