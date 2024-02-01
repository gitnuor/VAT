using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmContractVendor:ContractualProduction
{
    public int ContractVendorId { get; set; }
   
    public List<ContractualProductionProductDetail> ContractVendorProductDetailses { get; set; }
}