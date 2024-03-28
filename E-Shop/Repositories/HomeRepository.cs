using E_Shop.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Shop.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> Categories()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Clothing>> GetClothings(string searchTerm = "", int categoryId = 0)
        {
            searchTerm = searchTerm.ToLower();

            IEnumerable<Clothing> clothings = await (from clothing in _db.Clothings
                                                      join category in _db.Categories
                                                      on clothing.CategoryId equals category.Id
                                                      where string.IsNullOrWhiteSpace(searchTerm) || (clothing != null && clothing.Name.ToLower().StartsWith(searchTerm))
                                                      select new Clothing
                                                      {
                                                          Id = clothing.Id,
                                                          Image = clothing.Image,
                                                          Brand = clothing.Brand,
                                                          Name = clothing.Name,
                                                          CategoryId = clothing.CategoryId,
                                                          Price = clothing.Price,
                                                          CategoryName = category.Name
                                                      }
                                                     ).ToListAsync();

            if (categoryId > 0)
            {
                clothings = clothings.Where(c => c.CategoryId == categoryId).ToList();
            }

            return clothings;
        }
    }
}