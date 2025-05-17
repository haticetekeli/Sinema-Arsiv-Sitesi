using System.Threading.Tasks;
using System.Collections.Generic;
using SinemaArsivSitesi.Models;

namespace SinemaArsivSitesi.Services
{
    public interface IUserFavoriteMovieService
    {
        Task<bool> AddToFavorites(string userId, int movieId);
        Task<bool> RemoveFromFavorites(string userId, int movieId);
        Task<List<SinemaArsivSitesi.Models.Movie>> GetUserFavoriteMovies(string userId);
        Task<bool> IsMovieInFavorites(string userId, int movieId);
    }
}
