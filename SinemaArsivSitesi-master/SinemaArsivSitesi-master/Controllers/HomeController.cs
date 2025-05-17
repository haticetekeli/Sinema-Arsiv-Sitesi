using Microsoft.AspNetCore.Mvc;
using SinemaArsivSitesi.Models;
using SinemaArsivSitesi.Services.Category;
using SinemaArsivSitesi.Services.Movie;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SinemaArsivSitesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieService _movieService;
        private readonly ICategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, IMovieService movieService, ICategoryService categoryService)
        {
            _logger = logger;
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        public async Task<IActionResult> Index()
        {
            var filmler = await _movieService.GetAllMovies();
            var kategoriler = await _categoryService.GetAllCategories();

            ViewBag.FilmSayisi = filmler.Count();

            var viewModel = new HomeIndexViewModel
            {
                Filmler = filmler,
                Kategoriler = kategoriler
            };

            return View(viewModel);
        }

        public IActionResult Giris() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Sitemap() => View();
        public IActionResult Privacy() => View();
        public IActionResult Create() => View();

        public async Task<IActionResult> Details(int id)
        {
            var film = await _movieService.GetMovieById(id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
