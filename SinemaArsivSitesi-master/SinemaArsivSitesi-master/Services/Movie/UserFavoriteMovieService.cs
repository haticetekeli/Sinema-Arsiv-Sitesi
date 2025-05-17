using Microsoft.EntityFrameworkCore;
using SinemaArsivSitesi.Data.DbContext;
using SinemaArsivSitesi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinemaArsivSitesi.Services
{
    public class UserFavoriteMovieService : IUserFavoriteMovieService
    {
        private readonly ApplicationDbContext _context;

        public UserFavoriteMovieService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToFavorites(string userId, int movieId)
        {
            try
            {
                if (await IsMovieInFavorites(userId, movieId))
                {
                    return false;
                }

                var favorite = new UserFavoriteMovie
                {
                    UserId = userId,
                    MovieId = movieId,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    CreatedById = 1 // Admin ID örneði
                };

                await _context.UserFavoriteMovies.AddAsync(favorite);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveFromFavorites(string userId, int movieId)
        {
            try
            {
                var favorite = await _context.UserFavoriteMovies
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == movieId && !f.IsDeleted);

                if (favorite == null) return false;

                favorite.IsDeleted = true;
                favorite.UpdatedDate = DateTime.Now;
                favorite.UpdatedById = 1;

                _context.UserFavoriteMovies.Update(favorite);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<SinemaArsivSitesi.Models.Movie>> GetUserFavoriteMovies(string userId)
        {
            return await _context.UserFavoriteMovies
                .Include(f => f.Movie)
                    .ThenInclude(m => m.Category)
                .Where(f => f.UserId == userId && !f.IsDeleted)
                .Select(f => f.Movie)
                .ToListAsync();
        }

        public async Task<bool> IsMovieInFavorites(string userId, int movieId)
        {
            return await _context.UserFavoriteMovies
                .AnyAsync(f => f.UserId == userId && f.MovieId == movieId && !f.IsDeleted);
        }
    }
}
