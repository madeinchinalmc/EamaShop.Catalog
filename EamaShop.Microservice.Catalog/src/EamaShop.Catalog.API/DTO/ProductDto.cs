using EamaShop.Catalog.API.Respository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.DTO
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class ProductDTO : ProductCreateDTO
    {
        private readonly Product _product;
        /// <summary>
        /// init
        /// </summary>
        /// <param name="product"></param>
        public ProductDTO(Product product)
        {
            _product = product ?? throw new ArgumentNullException(nameof(product));

            if (_product.PictureUris != null)
            {
                PictureUris = JsonConvert.DeserializeObject<IEnumerable<string>>(_product.PictureUris);
            }
            if (_product.Properties != null)
            {
                Properties = JsonConvert.DeserializeObject<IEnumerable<string>>(_product.Properties);
            }
            if (_product.Specifications != null)
            {
                Specifications = _product.Specifications.Select(Selector).ToArray();
            }
        }

        private static SpecificationDTO Selector(Specification specification)
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            return new SpecificationDTO(specification);
        }
        /// <summary>
        /// 商品的Id
        /// </summary>
        public long ProductId => _product.Id;
        /// <summary>
        /// 商品的创建时间
        /// </summary>
        public DateTime CreateTime => _product.CreateTime;
        /// <summary>
        /// 最后的编辑时间
        /// </summary>
        public DateTime ModifiedTime => _product.ModifiedTime;
    }
}
