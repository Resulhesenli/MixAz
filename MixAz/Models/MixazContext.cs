using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MixAz.Models
{
    public partial class MixazContext : DbContext
    {
        public MixazContext()
        {
        }

        public MixazContext(DbContextOptions<MixazContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Follower> Followers { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<ProfilPhoto> ProfilPhotos { get; set; }
        public virtual DbSet<ProfilPost> ProfilPosts { get; set; }
        public virtual DbSet<ProfilPostLike> ProfilPostLikes { get; set; }
        public virtual DbSet<Sherh> Sherhs { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=wdb4.my-hosting-panel.com; Database=nurlans1_mixaz; User Id=nurlans1_mixaz; Password=k1@Vxo807");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName).HasMaxLength(30);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentWriteDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CommentPost)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CommentPostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Comments__Commen__72C60C4A");

                entity.HasOne(d => d.CommentUser)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CommentUserId)
                    .HasConstraintName("FK__Comments__Commen__71D1E811");
            });

            modelBuilder.Entity<Follower>(entity =>
            {
                entity.HasOne(d => d.FollowerFromUser)
                    .WithMany(p => p.FollowerFollowerFromUsers)
                    .HasForeignKey(d => d.FollowerFromUserId)
                    .HasConstraintName("FK__Followers__Follo__534D60F1");

                entity.HasOne(d => d.FollowerToUser)
                    .WithMany(p => p.FollowerFollowerToUsers)
                    .HasForeignKey(d => d.FollowerToUserId)
                    .HasConstraintName("FK__Followers__Follo__5441852A");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.GenderName).HasMaxLength(15);
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasOne(d => d.LikePost)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.LikePostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Likes__LikePostI__6B24EA82");

                entity.HasOne(d => d.LikeUser)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.LikeUserId)
                    .HasConstraintName("FK__Likes__LikeUserI__6A30C649");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.PhotoUrl)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.PhotoPost)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.PhotoPostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Photos__PhotoPos__59FA5E80");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.PostDescription).HasMaxLength(500);

                entity.Property(e => e.PostName).HasMaxLength(150);

                entity.Property(e => e.PostWriteDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.PostCategory)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.PostCategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Posts__PostCateg__571DF1D5");

                entity.HasOne(d => d.PostUser)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.PostUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Posts__PostUserI__46E78A0C");
            });

            modelBuilder.Entity<ProfilPhoto>(entity =>
            {
                entity.Property(e => e.ProfilPhotoUrl)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProfilPhotoPost)
                    .WithMany(p => p.ProfilPhotos)
                    .HasForeignKey(d => d.ProfilPhotoPostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ProfilPho__Profi__05D8E0BE");
            });

            modelBuilder.Entity<ProfilPost>(entity =>
            {
                entity.Property(e => e.ProfilPostDescription).HasMaxLength(500);

                entity.Property(e => e.ProfilPostPhotoUrl)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProfilPostWriteDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ProfilPostUser)
                    .WithMany(p => p.ProfilPosts)
                    .HasForeignKey(d => d.ProfilPostUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ProfilPos__Profi__6E01572D");
            });

            modelBuilder.Entity<ProfilPostLike>(entity =>
            {
                entity.HasOne(d => d.ProfilPostLikePost)
                    .WithMany(p => p.ProfilPostLikes)
                    .HasForeignKey(d => d.ProfilPostLikePostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ProfilPos__Profi__09A971A2");

                entity.HasOne(d => d.ProfilPostLikeUser)
                    .WithMany(p => p.ProfilPostLikes)
                    .HasForeignKey(d => d.ProfilPostLikeUserId)
                    .HasConstraintName("FK__ProfilPos__Profi__08B54D69");
            });

            modelBuilder.Entity<Sherh>(entity =>
            {
                entity.Property(e => e.SherhWriteDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.SherhPost)
                    .WithMany(p => p.Sherhs)
                    .HasForeignKey(d => d.SherhPostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Sherhs__SherhPos__628FA481");

                entity.HasOne(d => d.SherhUser)
                    .WithMany(p => p.Sherhs)
                    .HasForeignKey(d => d.SherhUserId)
                    .HasConstraintName("FK__Sherhs__SherhUse__619B8048");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.StatusName)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserMail).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(15);

                entity.Property(e => e.UserNickName)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword).HasMaxLength(30);

                entity.Property(e => e.UserProfilePhotoUrl).HasMaxLength(20);

                entity.Property(e => e.UserSurname).HasMaxLength(15);

                entity.HasOne(d => d.UserGender)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserGenderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Users__UserGende__4316F928");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Users__UserStatu__440B1D61");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
