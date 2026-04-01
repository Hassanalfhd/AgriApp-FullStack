using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agricultural_For_CV_DAL.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext _context;

        public ProductImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductsImages>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductsImages
                .Where(i => i.ProductId == productId)
                .OrderBy(i => i.ImageOrder)
                .ToListAsync();
        }

        public async Task<ProductsImages?> GetByIdAsync(int id)
        {
            return await _context.ProductsImages.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(ProductsImages image)
        {
            await _context.ProductsImages.AddAsync(image);
        }

        public async Task DeleteAsync(ProductsImages image)
        {
            _context.ProductsImages.Remove(image);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
