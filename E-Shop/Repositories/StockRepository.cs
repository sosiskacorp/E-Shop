using Microsoft.EntityFrameworkCore;

namespace E_Shop.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stock?> GetStockByClothingId(int clothingId) => await _context.Stocks.FirstOrDefaultAsync(s => s.ClothingId == clothingId);

        public async Task ManageStock(StockDTO stockToManage)
        {
            // if there is no stock for given clothing id, then add new record
            // if there is already stock for given clothing id, update stock's quantity
            var existingStock = await GetStockByClothingId(stockToManage.ClothingId);
            if (existingStock is null)
            {
                var stock = new Stock { ClothingId = stockToManage.ClothingId, Quantity = stockToManage.Quantity };
                _context.Stocks.Add(stock);
            }
            else
            {
                existingStock.Quantity = stockToManage.Quantity;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "")
        {
            var stocks = await (from clothing in _context.Clothings
                                join stock in _context.Stocks
                                on clothing.Id equals stock.ClothingId
                                into clothing_stock
                                from clothingStock in clothing_stock.DefaultIfEmpty()
                                where string.IsNullOrWhiteSpace(sTerm) || clothing.Name.ToLower().Contains(sTerm.ToLower())
                                select new StockDisplayModel
                                {
                                    ClothingId = clothing.Id,
                                    ClothingName = clothing.Name,
                                    Quantity = clothingStock == null ? 0 : clothingStock.Quantity
                                }
                                ).ToListAsync();
            return stocks;
        }

    }

    public interface IStockRepository
    {
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task<Stock?> GetStockByClothingId(int clothingId);
        Task ManageStock(StockDTO stockToManage);
    }
}