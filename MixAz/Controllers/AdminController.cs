using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MixAz.Models;
using System.IO;
using System.Linq;

namespace MixAz.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminController : Controller
    {
        private readonly MixazContext _sql;
        public AdminController(MixazContext sql)
        {
            _sql = sql;
        }
        public IActionResult Admin()
        {
            ViewBag.Category = _sql.Categories.ToList();
            var post = _sql.Posts.Where(x => x.PostShare == false).Include(x => x.Photos).ToList();
            _sql.SaveChanges();
            return View(post);
        }
        public IActionResult EditPost(int id)
        {
            ViewBag.Category = new SelectList(_sql.Categories.ToList(), "CategoryId", "CategoryName");
            return View(_sql.Posts.Include(x => x.Photos).SingleOrDefault(x => x.PostId == id));
        }
        public IActionResult DeletePost(int id)
        {
            _sql.Posts.Remove(_sql.Posts.SingleOrDefault(x => x.PostId == id));
            _sql.SaveChanges();
            return RedirectToAction("admin");
        }
        [HttpPost]
        public IActionResult UpdatePost(int id, Post paylasim, IFormFile[] MixPhoto)
        {
            Post oldpaylasim = _sql.Posts.SingleOrDefault(x => x.PostId == id);
            oldpaylasim.PostName = paylasim.PostName;
            oldpaylasim.PostDescription = paylasim.PostDescription;
            oldpaylasim.PostCategoryId = paylasim.PostCategoryId;
            foreach (var item in MixPhoto)
            {
                string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(item.FileName);
                using (Stream stream = new FileStream("wwwroot/img/" + filename, FileMode.Create))
                {
                    item.CopyTo(stream);
                }
                Photo p = new Photo();
                p.PhotoUrl = filename;
                p.PhotoPostId = id;
                _sql.Photos.Add(p);
                _sql.SaveChanges();
            }
            _sql.SaveChanges();
            return RedirectToAction("admin");
        }
        public IActionResult Apply(int id)
        {
            var a = _sql.Posts.Include(x => x.Photos).SingleOrDefault(x => x.PostId == id);
            a.PostShare = true;
            _sql.SaveChanges();
            return RedirectToAction("Admin");
        }
        public void RemoveImg(int id)
        {
            Photo p = _sql.Photos.SingleOrDefault(x => x.PhotoId == id);
            _sql.Photos.Remove(p);
            _sql.SaveChanges();
        }

    }
}
