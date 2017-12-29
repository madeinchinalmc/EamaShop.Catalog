using EamaShop.Catalog.API.Respository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.DTO
{
    /// <summary>
    /// 商品类目信息
    /// </summary>
    public class CategoryDTO : CategoryCreateDTO
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public long Id { get; }
        /// <summary>
        /// init
        /// </summary>
        /// <param name="category"></param>
        public CategoryDTO(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            Id = category.Id;
            Name = category.Name;
            StoreId = category.StoreId;
        }
    }
}
