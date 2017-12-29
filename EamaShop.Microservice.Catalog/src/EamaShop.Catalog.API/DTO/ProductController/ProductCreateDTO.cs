using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.DTO
{
    /// <summary>
    /// 商品的参数信息
    /// </summary>
    public class ProductCreateDTO
    {
        /// <summary>
        /// 商品所属的门店
        /// </summary>
        [Range(1, long.MaxValue)]
        public long StoreId { get; set; }
        /// <summary>
        /// 商品的名称
        /// </summary>
        [Required]
        [StringLength(80, MinimumLength = 5)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 商品的图片地址集合
        /// </summary>
        [Required]
        [MinLength(1)]
        public virtual IEnumerable<string> PictureUris { get; set; }
        /// <summary>
        /// 商品的属性信息
        /// </summary>
        public virtual IEnumerable<string> Properties { get; set; }
        /// <summary>
        /// 商品的规格信息
        /// </summary>
        [Required]
        [MinLength(1)]
        public virtual IEnumerable<SpecificationCreateDTO> Specifications { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        [StringLength(250)]
        public virtual string Description { get; set; }
        /// <summary>
        /// 商品所属的分类名称
        /// </summary>
        [Required]
        public virtual string CategoryName { get; set; }
    }
}
