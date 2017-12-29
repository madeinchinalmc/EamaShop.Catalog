using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.Infrastructures
{
    /// <summary>
    /// 当前规格的状态
    /// </summary>
    public enum SpecificationState
    {
        /// <summary>
        /// 售卖中
        /// </summary>
        OnSell,
        /// <summary>
        /// 已售罄
        /// </summary>
        NoStock,
        /// <summary>
        /// 已下架，不提供购买
        /// </summary>
        OffSell,
    }
}
