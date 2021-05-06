﻿using System;

namespace Delpin.Application.Contracts.v1.ProductGroups
{
    public class CreateProductGroupDto
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public Guid ProductCategoryId { get; set; }
    }
}
