using ContactManagerPro.Data;
using ContactManagerPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerPro.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var contacts = _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                contacts = contacts.Where(c =>
                    c.Nom.Contains(search) ||
                    c.Prenom.Contains(search) ||
                    c.Email.Contains(search) ||
                    c.Telephone.Contains(search) ||
                    c.Ville.Contains(search) ||
                    c.Profession.Contains(search)
                );
            }

            ViewData["Search"] = search;

            return View(await contacts.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["Companies"] = _context.Companies.ToList();
            ViewData["Categories"] = _context.Categories.ToList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Companies"] = _context.Companies.ToList();
                ViewData["Categories"] = _context.Categories.ToList();

                return View(contact);
            }

            contact.DateCreation = DateTime.Now;

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Favorites()
        {
            var contacts = await _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.Category)
                .Where(c => c.Favori == true)
                .ToListAsync();

            return View("Index", contacts);
        }
       

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
                return NotFound();


            ViewData["Companies"] = _context.Companies.ToList();
            ViewData["Categories"] = _context.Categories.ToList();

            return View(contact);
        }



        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contact contact)
        {
            if (id != contact.Id)
                return NotFound();


            if (!ModelState.IsValid)
            {
                ViewData["Companies"] = _context.Companies.ToList();
                ViewData["Categories"] = _context.Categories.ToList();

                return View(contact);
            }


            contact.DateCreation = DateTime.Now;

            _context.Update(contact);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();


            var contact = await _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.Category)
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.Id == id);


            if (contact == null)
                return NotFound();


            return View(contact);
        }



        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();


            var contact = await _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);


            if (contact == null)
                return NotFound();


            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
