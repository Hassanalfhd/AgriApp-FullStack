using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_Shared.Dtos.Products;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV_DAL.Interfaces
{
    public interface IProductRepository
    {

        Task<IEnumerable<Product>> GetAllAsync(bool includeRelated = false);
        Task<IEnumerable<Product>> GetAllAsyncWithMainImage(bool includeRelated = false);

        Task<IEnumerable<string>> GetSuggestionsAsync(string query);
        
        Task<Product?> GetAsync(int id, bool includeRelated = false);
        Task<bool> IsExistAsync(int id);
        Task UpdateAsync(Product product);
        Task  DeleteAsync(int id);

        Task<Product?> GetAsync(string name, bool includeRelated = false);
        Task AddAsync(Product product);

        Task<List<Product>> GetDeletedProductsWithImagesAsync();

        IQueryable<Product> GetQueryable();

        //Get Product By totalCount
        Task<(List<Product> items, int totalCount)> GetPagedProductsAsync(int page, int pageSize);
        Task<(List<Product> items, int totalCount)> GetAllUserProductsAsync(int userId, int page, int pageSize);


        //
        string GetProductName(int id);
    }
}
