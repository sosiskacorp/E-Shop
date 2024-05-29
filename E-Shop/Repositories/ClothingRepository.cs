using Microsoft.EntityFrameworkCore;

namespace E_Shop.Repositories
{
    public interface IClothingRepository
    {
        Task AddClothing(Clothing clothing);
        Task DeleteClothing(Clothing clothing);
        Task<Clothing?> GetClothingById(int id);
        Task<IEnumerable<Clothing>> GetClothings();
        Task UpdateClothing(Clothing clothing);
    }

    public class ClothingRepository : IClothingRepository
    {
        private readonly ApplicationDbContext _context;
        public ClothingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddClothing(Clothing clothing)
        {
            _context.Clothings.Add(clothing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClothing(Clothing clothing)
        {
            _context.Clothings.Update(clothing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClothing(Clothing clothing)
        {
            _context.Clothings.Remove(clothing);
            await _context.SaveChangesAsync();
        }

        public async Task<Clothing?> GetClothingById(int id) => await _context.Clothings.FindAsync(id);

        public async Task<IEnumerable<Clothing>> GetClothings() => await _context.Clothings.Include(a=>a.Category).ToListAsync();
    }
}