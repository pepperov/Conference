using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Alpha.Models;
using Alpha.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Controllers
{
    public class HomeController : Controller
    {
        private readonly AlphaDbContext _context;

        public IActionResult Index()
        {
            return RedirectToAction("Login", "Users");
        }

        public HomeController(AlphaDbContext context)
        {
            _context = context;
        }

        // GET: Home/Reservations
        public async Task<int> Reservations()
        {
            var count = await _context.Reservations.CountAsync(s => s.Status == Status.Idle);
            return count;
        }

    }
}
