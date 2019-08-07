using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Alpha.Models;
using Alpha.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Alpha.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Users");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
