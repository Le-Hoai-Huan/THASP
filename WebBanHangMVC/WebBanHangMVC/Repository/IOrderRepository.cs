using WebBanHangMVC.Models;

namespace WebBanHangMVC.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);

    }
}
