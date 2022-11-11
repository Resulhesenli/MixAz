using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MixAz.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MixAz.Controllers
{
    public class UserController : Controller
    {
        private readonly MixazContext _sql;
        public UserController(MixazContext sql)
        {
            _sql = sql;
        }
        public IActionResult Login()
        {
            ViewBag.Category = _sql.Categories.ToList();
            return View();
        }
        public IActionResult Registr()
        {
            ViewBag.Category = _sql.Categories.ToList();
            ViewBag.Genders = new SelectList(_sql.Genders.ToList(), "GenderId", "GenderName");
            return View();
        }
        [HttpPost]
        public IActionResult Registr(User u)
        {
            
            var user = _sql.Users.Any(x => x.UserNickName == u.UserNickName);
            if(!user)
            {

                if (u.UserGenderId == 1)
                {
                    u.UserProfilePhotoUrl = "Anonim.png";
                }
                else
                {
                    u.UserProfilePhotoUrl = "famale.png";
                }
                u.UserStatusId = 2;
                u.UserPrivate = false;
                _sql.Add(u);
                _sql.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("UserNickName", "Belə bir istifadəçi mövcuddur");
                return View("Registr");
            }
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}

            //_sql.Users.Add(u);
            //_sql.SaveChanges();
            //return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Moderator()
        {
            ViewBag.Category = _sql.Categories.ToList();
            ViewBag.Genders = new SelectList(_sql.Genders.ToList(), "GenderId", "GenderName");
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Moderator(User u)
        {

            var user = _sql.Users.Any(x => x.UserNickName == u.UserNickName);
            if (!user)
            {

                if (u.UserGenderId == 1)
                {
                    u.UserProfilePhotoUrl = "Anonim.png";
                }
                else
                {
                    u.UserProfilePhotoUrl = "famale.png";
                }
                u.UserStatusId = 1003;
                u.UserPrivate = false;
                _sql.Add(u);
                _sql.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("UserNickName", "Belə bir istifadəçi mövcuddur");
                return View("Registr");
            }
           
        }
        [HttpPost]
        public IActionResult Login(User u)
        {
            var getUser = _sql.Users.Include(x => x.UserStatus).SingleOrDefault(x => x.UserNickName == u.UserNickName && x.UserPassword == u.UserPassword);
            if (getUser != null)
            {
                var claims = new List<Claim>
                {   new Claim("Id", getUser.UserId.ToString()),
                    new Claim(ClaimTypes.Uri,getUser.UserProfilePhotoUrl),
                    new Claim(ClaimTypes.Role, getUser.UserStatus.StatusName)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var princitial = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, princitial, props).Wait();
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("Login", "User");
        }
    }
}
