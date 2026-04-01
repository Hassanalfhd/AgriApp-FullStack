using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;

namespace Agricultural_For_CV_DAL.Interfaces
{
    public interface IUserRepository
    {

        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
        
        Task<User?> GetByEmailAsync(string Email);
        
        Task<(List<User> items, int totalCount)> GetPagedUsersAsync(int page, int pageSize);
        
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);

        // --- Custom Queries ---
        Task<IEnumerable<User>> GetUsersByTypeAsync(int userType); // جميع المستخدمين من نوع محدد
        Task<bool> ExistsByEmailAsync(string email);              // تحقق من وجود البريد
        Task<bool> ExistsByUsernameAsync(string username);        // تحقق من وجود اسم المستخدم
    }
}
