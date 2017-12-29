using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.DTO
{
    /// <summary>
    /// 创建商品类目的参数信息
    /// </summary>
    public class CategoryCreateDTO
    {
        /// <summary>
        /// 商品类目的名称
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        /// <summary>
        /// 该类目所属的门店Id 对于管理员创建，该值为空
        /// </summary>
        public long? StoreId { get; set; }
    }
}
