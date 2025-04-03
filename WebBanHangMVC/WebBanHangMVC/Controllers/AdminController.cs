using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHangMVC.Models;
using WebBanHangMVC.Repository;

namespace WebBanHangMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IProductRepository productRepository, ICategoryRepository categoryRepository, IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        // Xóa sản phẩm
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Hiển thị form chỉnh sửa sản phẩm (trả về partial view cho modal)
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return PartialView("_EditProductModal", product);
        }

        // Lưu thay đổi sau khi chỉnh sửa
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetByIdAsync(product.Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                else
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;

                await _productRepository.UpadteAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return PartialView("_EditProductModal", product);
        }

        // Phương thức lưu hình ảnh (tương tự ProductController)
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName;
        }

        public async Task<IActionResult> Order()
        {
            var orders = await _orderRepository.GetAllAsync();
            return View(orders);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return PartialView("_OrderDetailsModal", order);
        }

        public async Task<IActionResult> User()
        {
            var users = await _userManager.Users.ToListAsync(); // Lấy tất cả người dùng
            var userRoles = new Dictionary<string, IList<string>>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles;
            }

            ViewBag.UserRoles = userRoles; // Truyền danh sách vai trò qua ViewBag
            return View(users);
        }

        // Hiển thị chi tiết người dùng (trả về partial view cho modal)
        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles; // Truyền vai trò qua ViewBag
            return PartialView("_UserDetailsModal", user);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return PartialView("_CreateProductModal", new Product());
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                await _productRepository.AddAsync(product); // Giả định có phương thức AddAsync
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return PartialView("_CreateProductModal", product);
        }
    }
}