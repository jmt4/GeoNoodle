using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAuth.Controllers
{
    public class PayStatisticsController : Controller
    {
        /* ASP.NET MVC uses the format below to determine what code to invoke */
        /* /[Controller]/[ActionName]/[Parameters] */
        /* https://localhost:44300/PayStatistics/Index */
        /* https://localhost:44300/PayStatistics/Welcome */
        // GET: PayStatistics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome(string name, int numTimes = 1)
        {
            ViewBag.Message = "Hello " + name;
            ViewBag.NumTimes = numTimes;
            return View();
        }
    }
}