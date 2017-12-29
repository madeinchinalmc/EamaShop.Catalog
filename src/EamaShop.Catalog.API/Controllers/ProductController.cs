using EamaShop.Catalog.API.DTO;
using EamaShop.Catalog.API.Respository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.Controllers
{
    /// <summary>
    /// 商品接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly ProductContext _context;
        /// <summary>
        /// 商品
        /// </summary>
        /// <param name="context"></param>
        public ProductController(ProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #region 创建商品的接口
        /// <summary>
        /// 创建商品的接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ProductDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody]ProductCreateDTO parameters)
        {
            if (parameters.PictureUris == null || parameters.PictureUris.Any(x => !Uri.IsWellFormedUriString(x, UriKind.Absolute)))
            {
                ModelState.TryAddModelError("PictureUris", "There has invalid uri string");
            }
            parameters.Specifications = parameters.Specifications.Where(x => x != null).ToArray();
            if (!parameters.Specifications.Any())
            {
                ModelState.TryAddModelError("Specifications", "规格必须至少有一个");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //transform
            Specification Selector(SpecificationCreateDTO dto)
            {
                return new Specification()
                {
                    CreateTime = DateTime.Now,
                    ModifiedTime = DateTime.Now,
                    Name = dto.Name,
                    Price = dto.Price,
                    State = Infrastructures.SpecificationState.OnSell,
                    StockCount = dto.StockCount,

                };
            }
            var product = new Product()
            {
                CategoryName = parameters.CategoryName,
                CreateTime = DateTime.Now,
                Description = parameters.Description,
                ModifiedTime = DateTime.Now,
                Name = parameters.Name,
                PictureUris = JsonConvert.SerializeObject(parameters.PictureUris),
                Properties = JsonConvert.SerializeObject(parameters.Properties),
                StoreId = parameters.StoreId,
                Specifications = parameters.Specifications.Where(x => x != null).Select(Selector).ToArray()
            };

            await _context.AddAsync(product, HttpContext.RequestAborted);

            await _context.SaveChangesAsync(HttpContext.RequestAborted);

            return CreatedAtAction(nameof(GetAsync), new { id = product.Id }, new ProductDTO(product));
        }
        #endregion

        #region 获取指定的商品详细信息
        /// <summary>
        /// 获取指定的商品详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ProductDTO))]
        public async Task<IActionResult> GetAsync(long id)
        {
            var product = await _context.Product
                .Include(x => x.Specifications)
                .SingleOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(new ProductDTO(product));
        }
        #endregion

        #region 编辑商品
        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Save([FromRoute]long id, [FromBody] ProductCreateDTO parameters)
        {
            var product = Transform(parameters);
            product.Id = id;
            var entry = _context.Entry(product);
            entry.Property(x => x.StoreId).IsModified = false;
            entry.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync(HttpContext.RequestAborted);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Product.Any(x => x.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            return Ok();
        }

        private Product Transform(ProductCreateDTO parameters)
        {
            if (parameters.PictureUris == null || parameters.PictureUris.Any(x => !Uri.IsWellFormedUriString(x, UriKind.Absolute)))
            {
                ModelState.TryAddModelError("PictureUris", "There has invalid uri string");
            }
            parameters.Specifications = parameters.Specifications.Where(x => x != null).ToArray();
            if (!parameters.Specifications.Any())
            {
                ModelState.TryAddModelError("Specifications", "规格必须至少有一个");
            }

            //transform
            Specification Selector(SpecificationCreateDTO dto)
            {
                return new Specification()
                {
                    CreateTime = DateTime.Now,
                    ModifiedTime = DateTime.Now,
                    Name = dto.Name,
                    Price = dto.Price,
                    State = Infrastructures.SpecificationState.OnSell,
                    StockCount = dto.StockCount,

                };
            }
            var product = new Product()
            {
                CategoryName = parameters.CategoryName,
                CreateTime = DateTime.Now,
                Description = parameters.Description,
                ModifiedTime = DateTime.Now,
                Name = parameters.Name,
                PictureUris = JsonConvert.SerializeObject(parameters.PictureUris),
                Properties = JsonConvert.SerializeObject(parameters.Properties),
                StoreId = parameters.StoreId,
                Specifications = parameters.Specifications.Where(x => x != null).Select(Selector)
            };

            return product;
        }
        #endregion

        #region 删除指定的商品信息
        /// <summary>
        /// 删除指定的商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]long id)
        {
            _context.Entry(new Product() { Id = id }).State = EntityState.Deleted;

            await _context.SaveChangesAsync(HttpContext.RequestAborted);

            return Ok();
        } 
        #endregion
    }
}
