using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.Respository
{
    /// <summary>
    /// 商家自定义的商品分类
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 分类的唯一标识Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 创建的门店Id
        /// </summary>
        [Range(1, long.MaxValue)]
        public long? StoreId { get; set; }
        /// <summary>
        /// 分类的名称
        /// </summary>
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Name { get; set; }
        /// <summary>
        /// 分类的类型
        /// </summary>
        public CategoryType Type { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后一次编辑时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }
    }
}
