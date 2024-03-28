using E_Shop.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchTerm = "", int categoryId = 0)
        {
            IEnumerable<Clothing> clothings = await _homeRepository.GetClothings(searchTerm, categoryId);
            IEnumerable<Category> categories = await _homeRepository.Categories();

            ClothingDisplayModel clothingModel = new ClothingDisplayModel
            {
                Clothings = clothings,
                Categories = categories,
                SearchTerm = searchTerm,
                CategoryId = categoryId
            };

            return View(clothingModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
