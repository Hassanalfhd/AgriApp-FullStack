using Agricultural_For_CV_DAL.Entities;

namespace Agricultural_For_CV_DAL.Interfaces
{
    public interface IProductImageRepository
    {
        Task<IEnumerable<ProductsImages>> GetByProductIdAsync(int productId);
        Task<ProductsImages?> GetByIdAsync(int id);
        Task AddAsync(ProductsImages image);
        Task DeleteAsync(ProductsImages image);
        Task SaveChangesAsync();
    }
}
