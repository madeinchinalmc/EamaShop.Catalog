using EamaShop.Catalog.API.Respository.EntityConfigs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.Respository
{
    /// <summary>
    /// product context
    /// </summary>
    public class ProductContext:DbContext
    {
        /// <summary>
        /// init
        /// </summary>
        /// <param name="options"></param>
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }
        /// <summary>
        /// category table
        /// </summary>
        public DbSet<Category> Category { get; set; }
        /// <summary>
        /// product table
        /// </summary>
        public DbSet<Product> Product { get; set; }
        /// <summary>
        /// specification table
        /// </summary>
        public DbSet<Specification> Specification { get; set; }
        /// <summary>
        /// configure entity
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new SpecificationConfig());
        }
    }
}
