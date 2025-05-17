using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinemaArsivSitesi.Models
{
    public class User : IdentityUser
    {
        [Required]
        [EmailAddress]
        public override string Email { get; set; } = string.Empty;


        public string? Name { get; set; }
        public string? Surname { get; set; }

        [NotMapped]
        public string? Role { get; set; }

        public virtual ICollection<UserFavoriteMovie> FavoriteMovies { get; set; } = new List<UserFavoriteMovie>();
        public virtual List<Comment> Comments { get; set; } = new();

    }
}
