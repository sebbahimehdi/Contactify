using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactManagerPro.Data;
using ContactManagerPro.Models;

namespace ContactManagerPro.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index(string searchString, string searchField = "all", bool? favorites = null, string sortBy = "name")
        {
            var contacts = from c in _context.Contacts select c;

            // Filter by favorites
            if (favorites.HasValue && favorites.Value)
            {
                contacts = contacts.Where(c => c.Favori);
            }

            // Search functionality
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                contacts = searchField switch
                {
                    "name" => contacts.Where(c =>
                        c.Nom.ToLower().Contains(searchString) ||
                        c.Prenom.ToLower().Contains(searchString)),
                    "email" => contacts.Where(c =>
                        c.Email.ToLower().Contains(searchString)),
                    "company" => contacts.Where(c =>
                        c.Entreprise.ToLower().Contains(searchString)),
                    "city" => contacts.Where(c =>
                        c.Ville.ToLower().Contains(searchString)),
                    "phone" => contacts.Where(c =>
                        c.Telephone.Contains(searchString)),
                    _ => contacts.Where(c =>
                        c.Nom.ToLower().Contains(searchString) ||
                        c.Prenom.ToLower().Contains(searchString) ||
                        c.Email.ToLower().Contains(searchString) ||
                        c.Entreprise.ToLower().Contains(searchString) ||
                        c.Ville.ToLower().Contains(searchString))
                };
            }

            // Sorting
            contacts = sortBy switch
            {
                "name-desc" => contacts.OrderByDescending(c => c.Nom).ThenByDescending(c => c.Prenom),
                "date-asc" => contacts.OrderBy(c => c.DateCreation),
                "date-desc" => contacts.OrderByDescending(c => c.DateCreation),
                "company" => contacts.OrderBy(c => c.Entreprise).ThenBy(c => c.Nom),
                "favorites" => contacts.OrderByDescending(c => c.Favori).ThenBy(c => c.Nom),
                _ => contacts.OrderBy(c => c.Nom).ThenBy(c => c.Prenom)
            };

            var viewModel = new ContactIndexViewModel
            {
                Contacts = await contacts.ToListAsync(),
                SearchString = searchString,
                SearchField = searchField,
                IsFavoritesFilter = favorites ?? false,
                SortBy = sortBy,
                TotalContacts = await _context.Contacts.CountAsync(),
                FavoriteCount = await _context.Contacts.CountAsync(c => c.Favori)
            };

            return View(viewModel);
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Prenom,Email,Telephone,Adresse,Entreprise,Profession,Ville,PhotoUrl,Notes,Favori,DateCreation")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.DateCreation = DateTime.Now;
                _context.Add(contact);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Contact {contact.Prenom} {contact.Nom} créé avec succès!";
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Prenom,Email,Telephone,Adresse,Entreprise,Profession,Ville,PhotoUrl,Notes,Favori,DateCreation")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Contact {contact.Prenom} {contact.Nom} modifié avec succès!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                string contactName = $"{contact.Prenom} {contact.Nom}";
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Contact {contactName} supprimé avec succès!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Contacts/ToggleFavorite/5
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            contact.Favori = !contact.Favori;
            _context.Update(contact);
            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, isFavorite = contact.Favori });
            }

            TempData["SuccessMessage"] = contact.Favori 
                ? $"{contact.Prenom} {contact.Nom} ajouté aux favoris!" 
                : $"{contact.Prenom} {contact.Nom} retiré des favoris!";

            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
