using EamaShop.Catalog.API.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.Respository
{
    /// <summary>
    /// 商品的规格信息
    /// </summary>
    public class Specification
    {
        /// <summary>
        /// 规格的唯一编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 商品规格的创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 商品规格的最后修改时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }
        /// <summary>
        /// 所属的商品Id
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 该规格的出售价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 该规格所剩余的库存
        /// </summary>
        public int StockCount { get; set; }
        /// <summary>
        /// 规格的名称 Eg. 加长版
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 规格的状态
        /// </summary>
        public SpecificationState State { get; set; }
    }
}
