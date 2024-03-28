namespace E_Shop
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Clothing>> GetClothings(string searchTerm = "", int categoryId = 0);
        Task<IEnumerable<Category>> Categories();
    }
}