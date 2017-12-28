using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.Respository
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 商品的唯一Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 所属的门店Id
        /// </summary>
        public long StoreId { get; set; }
        /// <summary>
        /// 商品的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品的图片地址列表 JSON Arrary Eg. ["http://www.domain.com/1.jpg","https://www.domain.com/2.png"]
        /// </summary>
        public string PictureUris { get; set; }
        /// <summary>
        /// 商品的规格信息列表
        /// </summary>
        public IEnumerable<Specification> Specifications { get; set; }
        /// <summary>
        /// 商品的属性信息集合对象 应该Parse为  Eg. [{"Name":"颜色","Values":["红","黄","蓝"]}]
        /// </summary>
        public string Properties { get; set; }
        /// <summary>
        /// 商品的描述介绍信息
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 商品所属的分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 商品的创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 商品的最后一次编辑时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }
    }
}
