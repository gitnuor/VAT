﻿using System;

namespace vms.entity.viewModels.ReportsViewModel;

public class vmSalesDetails
{
    public string InvoiceNo { get; set; }
    public string CustomerName { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}