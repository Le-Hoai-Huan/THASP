using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebBanHangMVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        [DisplayName("Tên danh mục")]
        public string Name { get; set; }
        public List<Product>? Products { get; set; }
    }
}
