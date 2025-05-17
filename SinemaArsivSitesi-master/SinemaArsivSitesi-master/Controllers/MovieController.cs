using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SinemaArsivSitesi.Data.DbContext;
using SinemaArsivSitesi.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SinemaArsivSitesi.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.Include(m => m.Category).ToListAsync();
            var vm = new HomeIndexViewModel
            {
                Filmler = movies
            };

            return View(vm);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Comments).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();

                ViewBag.Errors = errors;
                ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name", movie.CategoryId);
                return View(movie);
            }
            if (string.IsNullOrWhiteSpace(movie.Genre))
            {
                ModelState.AddModelError("Genre", "Tür (Genre) alanı zorunludur.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name", movie.CategoryId);
                return View(movie);
            }


            movie.CreatedDate = DateTime.Now;

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                movie.CreatedById = userId;
            }

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Film başarıyla eklendi!";
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name", movie.CategoryId);
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name", movie.CategoryId);
                return View(movie);
            }

            _context.Update(movie);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Film güncellendi.";
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.UserFavoriteMovies)
                .Include(m => m.RelatedMovies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            if (movie.UserFavoriteMovies.Any())
            {
                return BadRequest(new { message = "Film favorilerde olduğu için silinemez!" });
            }

            _context.RelatedMovies.RemoveRange(movie.RelatedMovies);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Film silindi.";
            return RedirectToAction("Index", "Home");
        }
    }
}
