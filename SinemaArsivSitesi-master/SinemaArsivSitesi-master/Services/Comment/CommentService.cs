using Microsoft.EntityFrameworkCore;
using SinemaArsivSitesi.Data;
using SinemaArsivSitesi.Models;
using SinemaArsivSitesi.Services.Comments;
using SinemaArsivSitesi.Data.DbContext;

namespace SinemaArsivSitesi.Services.Comments


{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Comment>> GetCommentsByFilmIdAsync(int filmId)
        {
            return await _context.Comments
                .Where(c => c.Id == filmId)
                .OrderByDescending(c => c.CreatedAt) 
                .ToListAsync();
        }

        public async Task<Models.Comment> GetCommentByIdAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                throw new KeyNotFoundException($"Comment with ID {id} not found.");
            }

            return comment;
        }


        public async Task AddCommentAsync(Models.Comment yorum)
        {
            await _context.Comments.AddAsync(yorum);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Models.Comment yorum)
        {
            _context.Comments.Update(yorum);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int id)
        {
            var yorum = await _context.Comments.FindAsync(id);
            if (yorum != null)
            {
                _context.Comments.Remove(yorum);
                await _context.SaveChangesAsync();
            }
        }
    }

}