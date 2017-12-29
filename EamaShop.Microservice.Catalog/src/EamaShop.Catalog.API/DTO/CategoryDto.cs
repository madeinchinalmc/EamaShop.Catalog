﻿using EamaShop.Catalog.API.Respository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Catalog.API.DTO
{
    public class CategoryDto : CategoryCreateDTO
    {
        public long Id { get; set; }

        public int Level { get; set; }

        public CategoryDto()
        {

        }
        public CategoryDto(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            Id = category.Id;
            Name = category.Name;
            StoreId = category.StoreId;
        }
    }
}
