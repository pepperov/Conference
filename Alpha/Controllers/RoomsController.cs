using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Alpha.Data;
using Alpha.Models;
using Microsoft.AspNetCore.Authorization;

namespace Alpha.Controllers
{
    public class RoomsController : Controller
    {
        private readonly AlphaDbContext _context;

        private List<Status> Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

        public RoomsController(AlphaDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var rooms = _context.Rooms.Include(r => r.Reservations);
            return View(await rooms.ToListAsync());
        }

        // GET: Rooms/Details/
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Room room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["Users"] = _context.Users.ToList();
            return View(room);
        }

        // GET: Rooms/Reservations
        [HttpGet]
        public ActionResult Reservations(int? id)
        {
            var res = _context.Reservations
                .Where(s => s.Status == Status.Approved && s.RoomId == id)
                .Include(r => r.Room).ToList();
            ViewData["Users"] = _context.Users.ToList();
            return PartialView("_Reservations", res);
        }

        // GET: Rooms/Add
        [Authorize(Roles = "Manager")]
        public IActionResult Add()
        {
            return View();
        }

        // POST: Rooms/Add
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Name,Descrtiption,Seats,Projector,Board")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Edit/
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Room room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,Descrtiption,Seats,Projector,Board")] Room room)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/Rooms/Details/{room.Id}");
            }
            return View(room);
        }

        // GET: Rooms/Remove/
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Room room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Rooms/Remove/
        [Authorize(Roles = "Manager")]
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            var res = await _context.Reservations.Where(s => s.RoomId == id).ToListAsync();
            foreach(Reservation reservation in res)
            {
                _context.Reservations.Remove(reservation);
            }

            Room room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
