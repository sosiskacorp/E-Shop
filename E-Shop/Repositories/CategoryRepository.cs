using Microsoft.EntityFrameworkCore;

namespace E_Shop.Repositories;

public interface ICategoryRepository
{
    Task AddCategory(Category genre);
    Task UpdateCategory(Category genre);
    Task<Category?> GetCategoryById(int id);
    Task DeleteCategory(Category genre);
    Task<IEnumerable<Category>> GetCategories();
}
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddCategory(Category genre)
    {
        _context.Categories.Add(genre);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateCategory(Category genre)
    {
        _context.Categories.Update(genre);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategory(Category genre)
    {
        _context.Categories.Remove(genre);
        await _context.SaveChangesAsync();
    }

    public async Task<Category?> GetCategoryById(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    
}