using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinemaArsivSitesi.Data.DbContext;
using SinemaArsivSitesi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SinemaArsivSitesi.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Add(int movieId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return RedirectToAction("Details", "Movie", new { id = movieId });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user == null)
                return RedirectToAction("Details", "Movie", new { id = movieId });

            var comment = new Comment
            {
                MovieId = movieId,
                UserId = user.Id,  
                Text = text,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Movie", new { id = movieId });
        }


        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound(new { message = "Yorum bulunamadı!" });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null || (comment.UserId != user.Id && !User.IsInRole("Admin")))
                return Unauthorized();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Movie", new { id = comment.MovieId });
        }
    }
}
