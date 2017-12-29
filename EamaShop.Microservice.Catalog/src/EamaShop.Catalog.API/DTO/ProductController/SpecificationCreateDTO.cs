using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.DTO
{
    /// <summary>
    /// 规格信息
    /// </summary>
    public class SpecificationCreateDTO
    {
        /// <summary>
        /// 规格的名称
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 规格的出售价格
        /// </summary>
        [Range(0.01, 999999999)]
        public decimal Price { get; set; }
        /// <summary>
        /// 规格的图片地址
        /// </summary>
        [Required]
        [MinLength(1)]
        public IEnumerable<string> PictureUris { get; set; }
        /// <summary>
        /// 规格的库存数量
        /// </summary>
        [Range(1, 999999)]
        public int StockCount { get; set; } = 9999;
    }
}
