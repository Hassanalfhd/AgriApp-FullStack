using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;

namespace Agricultural_For_CV_DAL.Interfaces
{
    public interface ICategoryRepository
    {

        Task<IEnumerable<Category>> GetAllAsync(bool includeRelated = false);

        Task<Category?> GetByIdAsync(int id, bool includeRelated = false);

        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);



        // --- Custom Queries ---
        Task<Category?> GetByNameAsync(string name);  // جلب الفئة حسب الاسم

    }
}
