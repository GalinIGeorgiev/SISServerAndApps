using SIS.Framework.ActionResults.Base;
using SIS.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sis.Demo2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
