﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ProductType
{
    public int ProductTypeId { get; set; }

    public string Name { get; set; }

    public string ShortName { get; set; }

    public bool IsPurchaseable { get; set; }

    public bool IsSellable { get; set; }

    public bool IsProductionable { get; set; }

    public bool IsInventory { get; set; }

    public bool IsUsedInProduction { get; set; }

    public bool IsTrading { get; set; }

    public bool IsMeasurable { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}