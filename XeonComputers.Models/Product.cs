﻿using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Enums;

namespace XeonComputers.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ProductType ProductType { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public decimal Price { get; set; }

        public decimal ParnersPrice { get; set; }

        public int ChildCategoryId { get; set; }
        public virtual ChildCategory ChildCategory { get; set; }

        public ICollection<Image> Images { get; set; }
    }
}