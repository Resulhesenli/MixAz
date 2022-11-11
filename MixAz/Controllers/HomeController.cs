using ImageMagick;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MixAz.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MixAz.Controllers
{
    public class HomeController : Controller
    {
        private readonly MixazContext _sql;
        public HomeController(MixazContext sql)
        {
            _sql = sql;
        }
        public IActionResult Index()
        {
            ViewBag.Category = _sql.Categories.ToList();
            ViewBag.Slide = _sql.Posts.Where(x=>x.PostShare == true).OrderByDescending(x => x.PostId).Include(x => x.Photos).Take(4).ToList();
            ViewBag.MaxView = _sql.Posts.OrderByDescending(x => x.PostViewCount).Include(x=>x.Photos).Take(4).ToList();
            var post = _sql.Posts.Where(x => x.PostShare == true).Include(x => x.Photos).Include(x => x.PostUser).Include(x =>x.Sherhs).Include(x => x.Likes).OrderByDescending(x => x.PostId).ToList();
            return View(post);
        }

        public IActionResult Profil(string UserNickName)
        {   
            if (UserNickName == null)
            {
                //ViewBag.Posts = _sql.ProfilPosts.Where(x => x.ProfilPostUserId == int.Parse(User.FindFirst("Id").Value));
                ViewBag.follower = _sql.Followers.Where(x => x.FollowerToUserId == int.Parse(User.FindFirst("Id").Value) && x.FollowerIsAccept == true).Count();
                ViewBag.following = _sql.Followers.Where(x => x.FollowerFromUserId == int.Parse(User.FindFirst("Id").Value) && x.FollowerIsAccept == true).Count();
                ViewBag.Post = _sql.ProfilPosts.Where(x => x.ProfilPostUserId == int.Parse(User.FindFirst("Id").Value)).Include(x => x.Comments).ThenInclude(x => x.CommentUser).Include(x=>x.ProfilPostLikes).Include(x=>x.ProfilPhotos).Include(x => x.ProfilPostUser).OrderByDescending(x => x.ProfilPostId).ToList();
                var user = _sql.Users.Include(x => x.FollowerFollowerToUsers.Where(x => x.FollowerFromUserId == int.Parse(User.FindFirst("Id").Value))).SingleOrDefault(x => x.UserId == int.Parse(User.FindFirst("Id").Value));
                return View(user);
            }
            else
            {
                ViewBag.follower = _sql.Followers.Include(x => x.FollowerToUser).Where(x => x.FollowerToUser.UserNickName == UserNickName && x.FollowerIsAccept == true).Count();
                ViewBag.following = _sql.Followers.Include(x => x.FollowerFromUser).Where(x => x.FollowerFromUser.UserNickName == UserNickName && x.FollowerIsAccept == true).Count();
                ViewBag.Post = _sql.ProfilPosts.Where(x => x.ProfilPostUser.UserNickName == UserNickName).Include(x => x.Comments).ThenInclude(x=>x.CommentUser).Include(x => x.ProfilPostLikes).Include(x=>x.ProfilPhotos).Include(x => x.ProfilPostUser).OrderByDescending(x => x.ProfilPostId).ToList();
                var user = _sql.Users.Include(x => x.FollowerFollowerToUsers.Where(x => x.FollowerFromUserId == int.Parse(User.FindFirst("Id").Value))).SingleOrDefault(x => x.UserNickName == UserNickName);
                return View(user);
            }   
        }
        [HttpPost]
        public IActionResult AddPost(ProfilPost profilpost, IFormFile[] ProfilPostPhotoUrl, string UserNickName)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _sql.ProfilPosts.Add(profilpost);
            profilpost.ProfilPostUserId = int.Parse(User.FindFirst("Id").Value);
            profilpost.ProfilPostWriteDate = DateTime.Now;
            _sql.SaveChanges();
            foreach (var item in ProfilPostPhotoUrl)
            {
                string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(item.FileName);
                using (Stream stream = new FileStream("wwwroot/img/" + filename, FileMode.Create))
                {
                    item.CopyTo(stream);
                }
                using (IMagickImage magickImage = new MagickImage("wwwroot/img/" + filename))
                {

                    magickImage.Quality = 20;
                    magickImage.Write("wwwroot/img/small/" + filename);
                }
                ProfilPhoto profilphotos = new ProfilPhoto();
                profilphotos.ProfilPhotoUrl = filename;
                profilphotos.ProfilPhotoPostId = profilpost.ProfilPostId;
                _sql.ProfilPhotos.Add(profilphotos);
                _sql.SaveChanges();

            }
            return RedirectToAction("Profil", new { UserNickName = UserNickName});
        }

     
        public IActionResult DeletePost(int id)
        {
            _sql.ProfilPosts.Remove(_sql.ProfilPosts.SingleOrDefault(x => x.ProfilPostId == id));
            _sql.SaveChanges();
            return RedirectToAction("Profil", new { id = User.FindFirst("Id").Value });
        }

        public IActionResult AddProfilComment(int id, string comment,string UserNickName)
        {
            Comment s = new Comment();
            s.CommentPostId = id;
            s.CommentUserId = int.Parse(User.FindFirst("Id").Value);
            s.CommentDescription = comment;
            _sql.Comments.Add(s);
            _sql.SaveChanges();
            return RedirectToAction("Profil", new { UserNickName = UserNickName });
        }
        public IActionResult DeleteProfilComment(int id)
        {
            var a = _sql.Comments.Include(x => x.CommentPost).ThenInclude(x=>x.ProfilPostUser).SingleOrDefault(x => x.CommentId == id);
            _sql.Comments.Remove(a);
            _sql.SaveChanges();
            return RedirectToAction("Profil", new { UserNickName = a.CommentPost.ProfilPostUser.UserName });
        }

        public IActionResult Xeber(int id)
        {
            var data = _sql.Posts.Where(x => x.PostId == id).FirstOrDefault();
            ViewBag.same = _sql.Posts.Include(x=>x.Photos).Where(x => x.PostCategoryId == data.PostCategoryId && x.PostId != id && x.PostShare == true).Take(4).OrderByDescending(x=>x.PostId).ToList();
            var post = _sql.Posts.Include(x =>x.Photos).Include(x => x.Sherhs).Include(x => x.Likes).SingleOrDefault(x =>x.PostId == id);
            ViewBag.Sherhs = _sql.Sherhs.Include(x => x.SherhUser).Where(x => x.SherhPostId == id).ToList();
            post.PostViewCount = post.PostViewCount + 1;
            _sql.SaveChanges();
            return View(post);
        }
       
        [HttpPost]
        public Sherh AddComment(Sherh shers, int id)
        {
            shers.SherhUserId = int.Parse(User.FindFirst("Id").Value);
            shers.SherhPostId = id;
            _sql.Sherhs.Add(shers);
            _sql.SaveChanges();
            var a = _sql.Sherhs.Include(x => x.SherhUser).SingleOrDefault(x => x.SherhId == shers.SherhId);
            return a;
        }
        public IActionResult DeleteComment(int id)
        {
            var a = _sql.Sherhs.SingleOrDefault(x => x.SherhId == id);
            _sql.Sherhs.Remove(a);
            _sql.SaveChanges();
            return RedirectToAction("Xeber", new { id = a.SherhPostId });
        }
        public IActionResult Melumatlarim()
        {
            var a = _sql.Users.SingleOrDefault(x => x.UserId == int.Parse(User.FindFirst("Id").Value));
            return View(a);
        }
        public IActionResult UpdateMelumatlarim(User user, IFormFile UserPhoto)
        {
            var a = _sql.Users.SingleOrDefault(x => x.UserId == int.Parse(User.FindFirst("Id").Value));
            a.UserName = user.UserName;
            a.UserSurname = user.UserSurname;
            a.UserMail = user.UserMail;
            if (UserPhoto != null)
            {
                string photoname = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(UserPhoto.FileName);

                using (Stream fileStream = new FileStream("wwwroot/img/" + photoname, FileMode.Create))
                {
                    UserPhoto.CopyTo(fileStream);
                }

                a.UserProfilePhotoUrl = photoname;
            }
            _sql.SaveChanges();
            return RedirectToAction("Melumatlarim");
        }
        public IActionResult Shifre()
        {
            var a = _sql.Users.SingleOrDefault(x => x.UserId == int.Parse(User.FindFirst("Id").Value));
            return View(a);
        }
        public IActionResult UpdatePass(string oldpass,string newpass,string replacepass,string id)
        {
            var a = _sql.Users.SingleOrDefault(x => x.UserId == int.Parse(User.FindFirst("Id").Value));
            if(newpass == replacepass)
            {
                if(a.UserPassword == oldpass)
                {
                    a.UserPassword = newpass;
                    _sql.SaveChanges();
                    return RedirectToAction("Login","User");
                }
                else
                {
                    ModelState.AddModelError("UserPassword", "Köhnə şifrə yalnışdır");
                }
            }
            else
            {
                ModelState.AddModelError("UserPassword", "Təkrar şifrə yalnışdır");
            }
            return View("Shifre",a);
        }
        [Authorize]
        public IActionResult Paylasimlar()
        {
            ViewBag.Category = new SelectList(_sql.Categories.ToList(), "CategoryId", "CategoryName");
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Paylasimlar(Post post, IFormFile[] PostPhotoUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _sql.Posts.Add(post);
            post.PostUserId = int.Parse(User.FindFirst("Id").Value);
            post.PostWriteDate = DateTime.Now;
            post.PostViewCount = 0;
            post.PostShare = false;
            _sql.SaveChanges();
            foreach (var item in PostPhotoUrl)
            {
                string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(item.FileName);
                using (Stream stream = new FileStream("wwwroot/img/" + filename, FileMode.Create))
                {
                    item.CopyTo(stream);
                }
                using (IMagickImage magickImage = new MagickImage("wwwroot/img/" + filename))
                {
                    
                    magickImage.Quality = 20;
                    magickImage.Write("wwwroot/img/small/" + filename);
                }
                Photo p = new Photo();
                p.PhotoUrl = filename;
                p.PhotoPostId = post.PostId;
                _sql.Photos.Add(p);
                _sql.SaveChanges();

            }
            return RedirectToAction("Index");
        }
       
        public IActionResult Dostlar()
        {
            return View();
        }
        public IActionResult Istekler()
        {
            return View();
        }
        public void PostLike(int id)
        {
            var oldcardlike = _sql.Likes.SingleOrDefault(x => x.LikePostId == id && x.LikeUserId == int.Parse(User.FindFirst("Id").Value));
            if (oldcardlike != null)
            {
                oldcardlike.IsLike = !oldcardlike.IsLike;
            }
            else
            {
                Like like = new Like();
                like.LikeUserId = int.Parse(User.FindFirst("Id").Value);
                like.IsLike = true;
                like.LikePostId = id;
                _sql.Likes.Add(like);
            }
            _sql.SaveChanges();
        }

        public void ProfilPostLike(int id)
        {
            var oldcardlike = _sql.ProfilPostLikes.SingleOrDefault(x => x.ProfilPostLikePostId == id && x.ProfilPostLikeUserId == int.Parse(User.FindFirst("Id").Value));
            if (oldcardlike != null)
            {
                oldcardlike.ProfilPostIsLike = !oldcardlike.ProfilPostIsLike;
            }
            else
            {
                ProfilPostLike profilpostlike = new ProfilPostLike();
                profilpostlike.ProfilPostLikeUserId = int.Parse(User.FindFirst("Id").Value);
                profilpostlike.ProfilPostIsLike = true;
                profilpostlike.ProfilPostLikePostId = id;
                _sql.ProfilPostLikes.Add(profilpostlike);
            }
            _sql.SaveChanges();
        }

        public List<User> Search(string q)
        {
            return _sql.Users.Where(x => x.UserNickName.ToLower().Contains(q.ToLower()) || x.UserName.ToLower().Contains(q.ToLower()) || x.UserSurname.ToLower().Contains(q.ToLower()) || x.UserProfilePhotoUrl.Contains(q)).ToList();
        }
        public IActionResult Forget()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendMessage(string UserNickName,string UserMail)
        {
            //var info = _sql.Users.SingleOrDefault(x => x.UserNickName == UserNickName);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse("mixazxeber@gmail.com");
            email.To.Add(MailboxAddress.Parse("mixazxeber@gmail.com"));
            email.Subject = "Şifrəni unutmuşam";
            var builder = new BodyBuilder();
            builder.TextBody = "salam";
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("mixazxeber@gmail.com", "R123456r*");
            smtp.Send(email);
            smtp.Disconnect(true);
            return RedirectToAction("Forget");
        }
    }
}

