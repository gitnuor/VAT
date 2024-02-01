using System;

namespace vms.entity.viewModels;

public class ProductionDataImportViewModel
{
    public string ProductionReciveId { get; set; }
    public string BatchNo { get; set; }
    public string ProductId { get; set; }
    public Decimal ReceiveQuantity { get; set; }
    public string MeasurementUnitId { get; set; }
    public DateTime ReceiveTime { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
    public bool IsContractual { get; set; }
    public string ContractualProductionId { get; set; }
    public string ContractualProductionChallanNo { get; set; }
       
}