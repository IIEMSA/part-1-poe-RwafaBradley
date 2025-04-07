using EventEaseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventEaseApp.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Event.Include(v => v.Venue).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "Locations");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event events)
        {
            if (ModelState.IsValid)
            {
                _context.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "Locations",events.VenueId);
            return View(events);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var evt = await _context.Event.FindAsync(id);
            if (evt == null) return NotFound();

            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "Locations", evt.VenueId);

            return View(evt);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Event evt)
        {
            if (id != evt.EventId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(evt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "Locations", evt.VenueId);

            return View(evt);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var evt = await _context.Event.FindAsync(id);
            if (evt == null) return NotFound();
            return View(evt);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evt = await _context.Event.FindAsync(id);
            _context.Event.Remove(evt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {

            var events = await _context.Event.Include(e => e.Venue)
            .FirstOrDefaultAsync(e => e.EventId == id);

            if (events == null)
            {

                return NotFound();

            }
            return View(events);
        }
    }
}
