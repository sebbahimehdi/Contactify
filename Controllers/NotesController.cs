using ContactManagerPro.Data;
using ContactManagerPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerPro.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            var notes = await _context.Notes
                .Include(n => n.Contact)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return View(notes);
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(n => n.Contact)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create(int? contactId)
        {
            ViewData["ContactId"] = contactId;
            ViewData["Contacts"] = _context.Contacts.ToList();
            return View();
        }

        // POST: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,ContactId")] Note note)
        {
            if (ModelState.IsValid)
            {
                _context.Add(note);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Note créée avec succès";

                if (note.ContactId > 0)
                {
                    return RedirectToAction("Details", "Contacts", new { id = note.ContactId });
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["Contacts"] = _context.Contacts.ToList();
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            ViewData["Contacts"] = _context.Contacts.ToList();
            return View(note);
        }

        // POST: Notes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,CreatedAt,ContactId")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Note mise à jour avec succès";

                    if (note.ContactId > 0)
                    {
                        return RedirectToAction("Details", "Contacts", new { id = note.ContactId });
                    }

                    return RedirectToAction(nameof(Details), new { id = note.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["Contacts"] = _context.Contacts.ToList();
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(n => n.Contact)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                int contactId = note.ContactId;
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Note supprimée avec succès";

                if (contactId > 0)
                {
                    return RedirectToAction("Details", "Contacts", new { id = contactId });
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
