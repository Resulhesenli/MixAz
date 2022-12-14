using Microsoft.AspNetCore.Mvc;
using MixAz.Models;
using System.Linq;

namespace MixAz.ViewComponents
{
    public class ResnavViewComponent : ViewComponent
    {
        private readonly MixazContext _sql;
        public ResnavViewComponent(MixazContext sql)
        {
            _sql = sql;
        }
        public IViewComponentResult Invoke()
        {
            User loginuser = _sql.Users.SingleOrDefault(x => x.UserId == int.Parse(HttpContext.User.FindFirst("Id").Value));
            return View(loginuser);
        }
    }
}
