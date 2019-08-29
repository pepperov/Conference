using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Alpha.Data;
using Alpha.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Alpha.Controllers
{
    public class UsersController : Controller
    {
        private readonly AlphaDbContext _context;

        public UsersController(AlphaDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = _context.Users.Include(s => s.Role);
            return View(await users.ToListAsync());
        }

        // GET: Users/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = await _context.Users
                .Include(s => s.Role)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            ViewData["Rooms"] = _context.Rooms.ToList();
            return View(user);
        }

        // GET: Rooms/Reservations
        [HttpGet]
        public ActionResult Reservations(int? id)
        {
            var res = _context.Reservations.Where(s => s.Status == Status.Approved && s.UserId == id).Include(r => r.Room);
            ViewData["Rooms"] = _context.Rooms.ToList();
            return PartialView("_Reservations", res);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(s => s.Email == model.Email);
                if (user == null)
                {
                    Role role = await _context.Roles.FirstOrDefaultAsync(s => s.Name == "Employee");
                    user = new User()
                    {
                        Name = model.Name,
                        Role = role,
                        Email = model.Email,
                        Password = model.Password
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    await Authenticate(user);
                    return RedirectToAction("Index", "Rooms");
                }
                else
                {
                    ModelState.AddModelError("", "User already exist");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.Include(s => s.Role).FirstOrDefaultAsync(s => s.Email == model.Email && s.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("Index", "Rooms");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                }
            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Users");
        }

        // GET: Users/Edit/
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId, Name, Email, Password")] User user)
        {
            user.Id = id;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/Users/Details/{id}");
            }
            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "Name");
            return View(user);
        }

        // GET: Users/Remove/
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Remove/
        [Authorize(Roles = "Manager")]
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            User user = await _context.Users.FindAsync(id);
            var res = await _context.Reservations.Where(s => s.UserId == id).ToListAsync();
            foreach (Reservation reservation in res)
            {
                _context.Reservations.Remove(reservation);
            }
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
