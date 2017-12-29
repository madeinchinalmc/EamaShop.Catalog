using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.DTO
{
    /// <summary>
    /// 修改商品类目的接口参数
    /// </summary>
    public class CategoryPutDTO : CategoryCreateDTO
    {
        /// <summary>
        /// 修改的商品类目Id
        /// </summary>
        [Range(1,long.MaxValue)]
        public long Id { get; set; }
    }
}
