namespace E_Shop.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int clothingId, int qty);
        Task<int> RemoveItem(int clothingId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> DoCheckout(CheckoutModel model);
    }
}