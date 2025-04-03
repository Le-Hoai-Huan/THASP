using WebBanHangMVC.Models;

namespace WebBanHangMVC.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);

        Task AddAsync(Product product);
        Task UpadteAsync (Product product);
        Task DeleteAsync(int id);

    }
}
