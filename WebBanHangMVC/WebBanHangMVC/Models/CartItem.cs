using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace WebBanHangMVC.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
