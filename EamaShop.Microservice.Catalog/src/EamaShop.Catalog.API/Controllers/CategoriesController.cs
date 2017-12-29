using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EamaShop.Catalog.API.Respository;
using Microsoft.AspNetCore.Authorization;
using EamaShop.Catalog.API.DTO;
using System.Security.Claims;
using System.Net;

namespace EamaShop.Catalog.API.Controllers
{
    /// <summary>
    /// 商品类目接口
    /// <para>商品类目不支持多层级，该类目仅用作商家创建商品时，可使用的预定义或自定义的分类名称</para>
    /// </summary>
    [Produces("application/json")]
    [Route("api/Categories")]
    public class CategoriesController : Controller
    {
        private readonly ProductContext _context;
        /// <summary>
        /// init
        /// </summary>
        /// <param name="context"></param>
        public CategoriesController(ProductContext context)
        {
            _context = context;
        }

        #region 修改类目信息
        /// <summary>
        /// 修改类目信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameters">修改的参数信息</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutCategory(long id, CategoryCreateDTO parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // mark a category;
            var temp = new Category() { Id = id, Name = parameters.Name, StoreId = parameters.StoreId };
            var entry = _context.Entry(temp);
            entry.State = EntityState.Modified;


            entry.Property(x => x.CreateTime).IsModified = false;
            if (User.IsInRole("Admin"))
            {
                entry.Property(x => x.StoreId).IsModified = false;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound(new { Message = "分类不存在，无法修改" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        #endregion

        #region 创建商品类目
        /// <summary>
        /// 创建商品类目
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostCategory([FromBody] CategoryCreateDTO category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isAdminitractor = User.IsInRole("Admin");
            var entity = new Category()
            {
                Name = category.Name,
                Type = isAdminitractor ? CategoryType.Predefinition : CategoryType.Custome,
                CreateTime = DateTime.Now,
                StoreId = category.StoreId
            };

            _context.Category.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = entity.Id }, entity);
        }
        #endregion

        #region 删除商品类目
        /// <summary>
        /// 删除商品类目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }
        #endregion

        private bool CategoryExists(long id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}