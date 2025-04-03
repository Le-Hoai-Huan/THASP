﻿using Microsoft.EntityFrameworkCore;
using WebBanHangRestApi.model;

namespace WebBanHangRestApi.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task AddProductAsync(Product product)
        {
              _context.Products.Add(product);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
