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
        public async Task<IActionResult> Index()
        {
            var rooms = _context.Rooms.Include(r => r.Reservations);
            return View(await rooms.ToListAsync());
        }

        // GET: Rooms/Details/
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
//            ViewData["Status"] = new SelectList(Statuses, Statuses.ToString());
            ViewData["Status"] = Statuses;
            return View(room);
        }

        // GET: Rooms/Reservations
        [HttpGet]
        public ActionResult Reservations(int? id)
        {
            //Room room = _context.Rooms.Include(r => r.Reservations).FirstOrDefault(m => m.Id == id);
            //var res = room.Reservations.Where(s => s.Status == Status.Approved);
            var res = _context.Reservations.Where(s => s.Status == Status.Approved && s.RoomId == id).Include(r => r.Room);

            ViewData["Users"] = _context.Users.ToList();
            ViewData["Status"] = Statuses;

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
        public async Task<IActionResult> Edit(int id, [Bind("Name,Descrtiption,Seats,Projector,Board")] Room room)
        {
            room.Id = id;
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
                return Redirect($"/Rooms/Details/{id}");
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
