using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinemaArsivSitesi.Models
{
    public class Movie : BaseEntity
    {
        [Required, StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string PosterUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string Genre { get; set; } = string.Empty;

        [StringLength(50)]
        public string Language { get; set; }

        [Range(0, 10)]
        [Column(TypeName = "decimal(3,1)")]
        public decimal IMDBRating { get; set; }



        [Range(1900, 2100)]
        public int Year { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(500)]
        public string VideoUrl { get; set; }

        [StringLength(100)]
        public string Director { get; set; }

        [StringLength(100)]
        public string Duration { get; set; }

        [NotMapped]
        public ICollection<string> Actors { get; set; } = new List<string>();

        public int? CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        public virtual ICollection<UserFavoriteMovie> UserFavoriteMovies { get; set; } = new List<UserFavoriteMovie>();

        public virtual ICollection<RelatedMovie> RelatedMovies { get; set; } = new List<RelatedMovie>();

        public virtual ICollection<RelatedMovie> RelatedToMovies { get; set; } = new List<RelatedMovie>();
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
