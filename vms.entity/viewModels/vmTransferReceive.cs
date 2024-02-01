using System;
using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmTransferReceive
{
    public int TransferSalesId { get; set; }
    public int OrganizationId { get; set; }
    public string InvoiceNo { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int PurchaseReasonId { get; set; }
    public DateTime DeliveryDate { get; set; }
    public bool IsComplete { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
    public List<Content> ContentJson { get; set; }

}