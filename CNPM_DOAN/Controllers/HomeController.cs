using CNPM_DOAN.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CNPM_DOAN.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult changeLanguage(string language)
        {
            Console.WriteLine(language);
            if (!String.IsNullOrEmpty(language))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }
            HttpCookie cookie = new HttpCookie("Languages");
            cookie.Value = language;
            Response.Cookies.Add(cookie);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
        
        public ActionResult Index(string language)
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult lienhe()
        {
            return View();
        }
        public ActionResult gioithieu()
        {
            return RedirectToAction("Index","Home");
        }
    }
}