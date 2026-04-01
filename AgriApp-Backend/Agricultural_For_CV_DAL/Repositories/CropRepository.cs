using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agricultural_For_CV_DAL.Repositories
{
    public class CropRepository: ICropRepository
    {

        private readonly AppDbContext _context;

        public CropRepository(AppDbContext context) { 
            this._context= context;

        }


        public async Task<IEnumerable<Crop>> GetAllAsync(bool includeRelated = false)
        {
            var query =_context.Crops.AsNoTracking();

            if (includeRelated)
            {
                query = query.Include(c => c.Owner).Include(c => c.Category);
            }
            
            return await query.OrderByDescending(c => c.CreatedAt).ToListAsync(); 
        }


        public async Task<Crop?> GetByIdAsync(int id, bool includeRelated = false)
        {
            var query = _context.Crops.AsNoTracking().Where(c=>c.Id ==id);

            if (includeRelated)
            {
                query = query.Include(c => c.Owner).Include(c => c.Category);

            }

            return await query.FirstOrDefaultAsync();

        }

        public async Task<bool> IsCropExist(int id)
        {
            var query = _context.Crops.AsNoTracking().Where(c => c.Id == id);
            if (query.Any())
            {
                return true;
            }
            return false;

        }

        public async Task AddAsync(Crop crop)
        {
            await _context.Crops.AddAsync(crop);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Crop crop)
        {
            _context.Crops.Update(crop);

            await _context.SaveChangesAsync();  

        }
        
        public async Task DeleteAsync(int id)
        {

            var crop = await _context.Crops.FindAsync(id);

            if(crop != null)
            {
                 _context.Crops.Remove(crop);
                await _context.SaveChangesAsync();
            }
        }



        // --- Custom Query ---
        public async Task<IEnumerable<Crop>?> GetCropsByUserIdAsync(int userId)
        {

            var crops = await _context.Crops
                                 .Where(c => c.OwnerId == userId)
                                 .Include(c => c.Category)
                                 .AsNoTracking()
                                 .ToListAsync();


            if (crops == null || crops.Count <= 0)

                return null;


            return crops;

        }

        public async Task<IEnumerable<Crop>?> GetCropsByCategoryIdAsync(int categoryId)
        {


            var crops = await _context.Crops
                                 .Where(c => c.CategoryId == categoryId)
                                 .Include(c => c.Owner)
                                 .AsNoTracking()
                                 .ToListAsync();


            if (crops == null || crops.Count <= 0)
       
                return null;
        
            
            return crops;


        }

        public async Task<IEnumerable<Crop>?> SearchCropsByNameAsync(string name)
        {
                
            var crops = await _context.Crops
                                 .Where(c => c.Name.Contains(name))
                                 .AsNoTracking()
                                 .ToListAsync();

            if (crops == null || crops.Count <= 0)
                return null;

            return crops;



        }



    }
}
