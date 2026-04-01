using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agricultural_For_CV_DAL.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            this._context = context;

        }

        public async Task<IEnumerable<Category>> GetAllAsync(bool includeRelated = false)
        {
            var query = _context.Categories.AsNoTracking();

            if (includeRelated)
            {
                query = query.Include(c => c.Crops);
            }

            return await query.ToListAsync();
        }


        public async Task<Category?> GetByIdAsync(int id, bool includeRelated = false)
        {
            var query = _context.Categories.AsNoTracking();

            if (includeRelated)
            {
                query = query.Include(c => c.Crops);
            }

            return await query.FirstOrDefaultAsync(c=>c.Id == id);
        }


        public async Task AddAsync(Category category)
        {

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {

            var category =await  _context.Categories.FindAsync(id);

            if(category!=null)
            {
                _context.Categories.Remove(category);

                await _context.SaveChangesAsync();
            }

        }


        // --- Custom Queries ---
        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.Name == name);
        }


    }
}
