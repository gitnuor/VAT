﻿using System;
using System.Collections.Generic;
using vms.entity.models;

namespace vms.Models;

public class vmProductionReceive
{
    public long ProductionReceiveId { get; set; }
    public string BatchNo { get; set; }
    public int OrganizationId { get; set; }
    public int? ProductionId { get; set; }
    public int ProductId { get; set; }
    public decimal ReceiveQuantity { get; set; }
    public int MeasurementUnitId { get; set; }
    public DateTime ReceiveTime { get; set; }
    public decimal MaterialCost { get; set; }
    public bool IsActive { get; set; }
    public int? CreatedBy { get; set; }
    public bool? IsContractual { get; set; }
    public int? ContractualProductionId { get; set; }
    public string ContractualProductionChallanNo { get; set; }
    public DateTime? CreatedTime { get; set; }
    public List<BillOfMaterial> ProductionReceiveDetailList { get; set; }
    public List<Content> ContentInfoJsonTest { get; set; }
    public List<ContentInfo> ContentInfoJson { get; set; }
}