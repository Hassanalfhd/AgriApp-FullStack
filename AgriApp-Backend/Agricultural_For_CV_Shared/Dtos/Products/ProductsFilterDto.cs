using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Dtos.Products
{
    /// <summary>
    /// Filters for product listing
    /// </summary>
    public class ProductsFilterDto
    {
        /// <summary>
        /// Search term for product name
        /// </summary>
        /// <example>tomato</example>
        public string? Search { get; set; }

        /// <summary>
        /// Minimum price filter
        /// </summary>
        /// <example>500</example>
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// Maximum price filter
        /// </summary>
        /// <example>2000</example>
        public decimal? MaxPrice { get; set; }
    }

}
