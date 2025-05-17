using System.ComponentModel.DataAnnotations.Schema;

namespace SinemaArsivSitesi.Models
{
    public class RelatedMovie
    {
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        public int RelatedMovieId { get; set; }

        [ForeignKey("RelatedMovieId")]
        public Movie Related { get; set; }
    }
}
