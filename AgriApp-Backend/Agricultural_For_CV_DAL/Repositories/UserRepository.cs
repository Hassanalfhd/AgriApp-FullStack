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
    public class UserRepository: IUserRepository
    {

        private readonly AppDbContext _context;


        public UserRepository(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.OrderByDescending(u=>u.CreatedAt).AsNoTracking().ToListAsync();
        }


        public async Task<User?>GetByIdAsync(int id)
        {

            return await _context.Users
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(u => u.Id == id);

        }

        public async Task<(List<User>items, int totalCount)>GetPagedUsersAsync(int page, int pageSize)
        {
            var skipValue = (page - 1) * pageSize;
            var totalCount = await _context.Users.CountAsync();

            var items = await _context.Users
                        .OrderBy(u => u.fullName)
                        .Skip(skipValue)
                        .Take(pageSize)
                        .ToListAsync();

           
            return (items, totalCount);
        }



        public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {

            return await _context.Users
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }

        public async Task<User?> GetByEmailAsync(string Email)
        {

            return await _context.Users
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(u => u.Email == Email);
        }



        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();  
        }


        public async Task UpdateAsync(User user)
        {
             _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

        }


        // --- Custom Queries ---
        public async Task<IEnumerable<User>> GetUsersByTypeAsync(int userType)
        {
            return await _context.Users
                                 .Where(u => u.UserType == userType)
                                 .AsNoTracking()
                                 .ToListAsync();
        }


        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }


        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }


    }
}
