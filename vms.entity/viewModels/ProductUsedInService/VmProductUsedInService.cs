using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vms.entity.viewModels.ProductUsedInService
{
    public class VmProductUsedInService
    {
        public string EncryptedId { get; set; }
        public int ProductUsedInServiceId { get; set; }
        public string BranchName { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string MeasurementUnitName { get; set; }
        public string ReferenceKey { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public int ModifiedBy { get; set; }
    }
}
