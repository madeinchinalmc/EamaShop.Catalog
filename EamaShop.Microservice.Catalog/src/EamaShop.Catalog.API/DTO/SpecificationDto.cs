using EamaShop.Catalog.API.Infrastructures;
using EamaShop.Catalog.API.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.DTO
{
    /// <summary>
    /// 商品的规格信息
    /// </summary>
    public class SpecificationDTO : SpecificationCreateDTO
    {
        private readonly Specification _specification;
        /// <summary>
        /// 初始化商品的规格信息
        /// </summary>
        /// <param name="specification"></param>
        public SpecificationDTO(Specification specification)
        {
            _specification = specification ?? throw new ArgumentNullException(nameof(specification));
            Price = _specification.Price;
            StockCount = _specification.StockCount;
            Name = _specification.Name;
        }
        /// <summary>
        /// 规格的Id
        /// </summary>
        public long SpecificationId => _specification.Id;
        /// <summary>
        /// 商品的规格状态
        /// </summary>
        public SpecificationState SpecificationState => _specification.State;
    }
}
