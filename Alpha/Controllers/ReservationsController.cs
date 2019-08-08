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
    public class ReservationsController : Controller
    {
        private readonly AlphaDbContext _context;

        private List<Status> Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

        public ReservationsController(AlphaDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var res = _context.Reservations;
            return View(await res.ToListAsync());
        }

        // GET: Reservations/Add
        public IActionResult Add(int? id)
        {
            ViewData["Status"] = new SelectList(Statuses, Statuses.ToString());
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", id);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Reservations/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Start,End,UserId,RoomId")] Reservation reservation, int? roomid)
        {
            reservation.Status = Status.Idle;
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();

                if (roomid == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Redirect($"/Rooms/Details/{roomid}");
                }
            }
            ViewData["Status"] = new SelectList(Statuses, Statuses.ToString());
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", reservation.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Edit/
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id, int? roomid)
        {
            if (id == null)
            {
                return NotFound();
            }
            Reservation reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["Status"] = new SelectList(Statuses, Statuses.ToString());
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", reservation.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", reservation.UserId);
            ViewData["Users"] = _context.Users.ToList();
            ViewData["Rooms"] = _context.Rooms.ToList();
            return View(reservation);
        }

        // GET: Reservations/Reservations
        [Authorize(Roles = "Manager")]
        public IActionResult Reservations()
        {
            var res = _context.Reservations
                .Include(s => s.Room)
                .Include(s => s.User)
                .OrderBy(s => s.Status);
            ViewData["Status"] = Statuses;
            ViewData["Users"] = _context.Users.ToList();
            ViewData["Rooms"] = _context.Rooms.ToList();
            return PartialView("_Reservations", res);
        }

        // GET: Reservations/Aproove
        public async Task<Status> Aproove(int? id)
        {
            Reservation reservation = await _context.Reservations.FirstOrDefaultAsync(s => s.Id == id);

            if (ModelState.IsValid)
            {
                reservation.Status = Status.Approved;
                _context.Update(reservation);
                await _context.SaveChangesAsync();
            }
            return reservation.Status;
        }


        // POST: Reservations/Edit/
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Status,Start,End,UserId,RoomId")] Reservation reservation, int? roomid)
        {
            reservation.Id = id;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (roomid == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            ViewData["Status"] = new SelectList(Statuses, Statuses.ToString());
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", reservation.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", reservation.UserId);
            return View(reservation);
        }


        // GET: Reservations/Remove/
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Reservation reservation = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Remove/
        [Authorize(Roles = "Manager")]
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            Reservation reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
