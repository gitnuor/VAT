﻿using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel;

public class spGet6P3View
{
    [Key]
    public int SalesId { get; set; }
    public string InvoiceNo { get; set; }
    public string CustomerName { get; set; }
    public DateTime SalesDate { get; set; }
}