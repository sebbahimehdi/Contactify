using ContactManagerPro.Data;
using ContactManagerPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ContactManagerPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dashboardData = new DashboardViewModel
            {
                TotalContacts = _context.Contacts.Count(),
                FavoriteContacts = _context.Contacts.Count(c => c.Favori),
                RecentContacts = _context.Contacts
                    .Include(c => c.Company)
                    .Include(c => c.Category)
                    .OrderByDescending(c => c.DateCreation)
                    .Take(5)
                    .ToList(),
                CompaniesCount = _context.Companies.Count(),
                CategoriesCount = _context.Categories.Count(),
                NotesCount = _context.Notes.Count(),
                CitiesCount = _context.Contacts.Select(c => c.Ville).Distinct().Count()
            };

            return View(dashboardData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
