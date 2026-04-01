using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.Products;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Agricultural_For_CV_DAL.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Product>> GetAllAsync(bool includeRelated = false)
        {

            var query = _context.Products
                 .AsNoTracking()
                 .Where(p => !p.IsDeleted && p.QuantityInStock >0);
            

            if (includeRelated)
            {
                query = query
                    .Include(p => p.Crops)
                    .Include(p => p.User)
                    .Include(p => p.QuantityTypes);
            }

            
            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }


        public async Task<IEnumerable<Product>> GetAllAsyncWithMainImage(bool includeRelated = false)
        {
  
            var query = _context.Products
                .AsNoTracking()
                     .Where(p => !p.IsDeleted);

            if (includeRelated)
            {
                query = query
                    .Include(p => p.Crops)
                    .Include(p => p.ProductsImages.Where(img => img.ImageOrder == 1))
                    .Include(p => p.User)
                    .Include(p => p.QuantityTypes);
            }


            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }


        public async Task<(List<Product> items, int totalCount)> GetPagedProductsAsync(int page, int pageSize)
        {
            var totalCount = await _context.Products.Where(p=>!p.IsDeleted).CountAsync();
            var items = await _context.Products
                .Where(p=>!p.IsDeleted && p.QuantityInStock >= 1)
                .OrderByDescending(p => p.CreatedAt)
                .Include(p => p.User)
                .Include(p => p.ProductsImages.Where(img => img.ImageOrder == 1))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

           

            return (items, totalCount);
        }


        public async Task<(List<Product> items, int totalCount)> GetAllUserProductsAsync(int userId, int page, int pageSize)
        {

            var totalCount = await _context.Products.Where(p=>!p.IsDeleted&& p.CreatedBy == userId).CountAsync();
            var items = await _context.Products
                .Where(p => !p.IsDeleted && p.CreatedBy == userId)
                .OrderBy(p => p.CreatedAt)
                .Include(p => p.User)
                .Include(p => p.ProductsImages.Where(img => img.ImageOrder == 1))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();




            return (items, totalCount);
            

        }


        public async Task<IEnumerable<string>> GetSuggestionsAsync(string query)
        {
            
            return await _context.Products
                .Where(p => p.Name.Contains(query) && !p.IsDeleted)
                .Select(p => p.Name)
                .Take(10) // Number of suggestion
                .ToListAsync();
        }


        public async Task<Product?> GetAsync(int id, bool includeRelated = false)
        {
            var query = _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id&& !p.IsDeleted);

            if (includeRelated)
            {
                query = query
                    .Include(p => p.Crops)
                    .Include(p => p.User)
                    .Include(p => p.QuantityTypes);
            }

            

            query = query.Include(p => p.ProductsImages);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Product?> GetAsync(string name, bool includeRelated = false)
        {
            IQueryable<Product> query = _context.Products
                .AsNoTracking()
                .Where(p => p.Name == name && !p.IsDeleted)
                .Include(p => p.ProductsImages.Where(img=>img.ImageOrder ==1 ));

            if (includeRelated)
            {
                query = query
                    .Include(p => p.Crops)
                    .Include(p => p.User)
                    .Include(p => p.QuantityTypes);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Product product)
        {
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null)
                throw new Exception($"Product with ID {product.Id} not found.");

            // تحديث خصائص المنتج فقط
            _context.Entry(existingProduct).CurrentValues.SetValues(product);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.
                Include(p=>p.ProductsImages)
                .FirstOrDefaultAsync(p=>p.Id == id);

            if(product != null)
            {
                _context.Remove(product);
                await _context.SaveChangesAsync();
            }

        }


        public async Task<bool> IsExistAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product!= null)
                return true;

            return false;
        }


        public async Task<List<Product>> GetDeletedProductsWithImagesAsync()
        {
            return await _context.Products
                .Where(p => p.IsDeleted)
                .Include(p => p.ProductsImages)
                .ToListAsync();
        }



        public IQueryable<Product> GetQueryable()
        {
            return _context.Products.AsNoTracking();
        }


        public string GetProductName(int id) => _context.Products.Find(id).Name ?? "unknown";

    }


}
