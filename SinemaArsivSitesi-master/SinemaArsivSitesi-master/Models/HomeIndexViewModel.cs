using System.Collections.Generic;

namespace SinemaArsivSitesi.Models
{
    public class HomeIndexViewModel
    {
        public List<Movie> Filmler { get; set; } = new List<Movie>();
        public List<Category> Kategoriler { get; set; } = new List<Category>();
    }
}
