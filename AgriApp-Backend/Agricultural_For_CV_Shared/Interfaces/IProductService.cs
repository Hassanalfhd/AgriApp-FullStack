using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.Products;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Results;

namespace Agricultural_For_CV_Shared.Interfaces
{
    public interface IProductService
    {
        Task<Result<PagedResult<ProductsResponseDto>>> GetProductsAsync(int page, int pageSize);

        Task<Result<PagedResult<ProductsResponseDto>>> GetAllUserProductsAsync(int userId, int page, int pageSize);
        Task<Result<IEnumerable<ProductsResponseDto>>>GetAllProductsAsync();
        Task<Result<ProductsResponseDto>>AddProductAsync(ProductToCreate createProductDto);
        Task<Result<IEnumerable<string>>> GetProductSuggestionsAsync(string query);

        Task<Result<bool>> UpdateProductStatusAsync(int id, enProductStatus productStatus);

        Task<Result<bool>> DeleteProduct(int id);
        Task<Result<ProductResponseDto>> GetProductAsync(string name);
        Task<Result<ProductResponseDto>> GetProductByIdAsync(int id);
        Task<Result<ProductsResponseDto>> UpdateProductAsync(ProductToUpdate updateProduct);

        Task<List<ProductsResponseDto>> GetProductsAsync(ProductsFilterDto filter);




    }
}
