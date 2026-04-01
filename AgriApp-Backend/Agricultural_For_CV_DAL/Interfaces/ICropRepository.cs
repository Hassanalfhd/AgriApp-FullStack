using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;

namespace Agricultural_For_CV_DAL.Interfaces
{
    public interface ICropRepository
    {

        Task<IEnumerable<Crop>> GetAllAsync(bool includeRelated = false);
        Task<Crop?> GetByIdAsync(int id, bool includeRelated = false);
        Task<bool> IsCropExist(int id);
        Task AddAsync(Crop crop);
        Task UpdateAsync(Crop crop);
        Task DeleteAsync(int id);


        // --- Custom Queries ---
        Task<IEnumerable<Crop>?> GetCropsByUserIdAsync(int userId);        // كل المحاصيل لمستخدم معين
        Task<IEnumerable<Crop>?> GetCropsByCategoryIdAsync(int categoryId); // كل المحاصيل لفئة محددة
        Task<IEnumerable<Crop>?> SearchCropsByNameAsync(string name);
    

    }
}
