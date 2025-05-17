using Microsoft.AspNetCore.Identity;

namespace SinemaArsivSitesi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public virtual ICollection<UserFavoriteMovie> FavoriteMovies { get; set; } = new List<UserFavoriteMovie>();
    }
}
