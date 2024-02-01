using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vms.entity.viewModels
{
    public class VmViewProductionReceive
    {
        public int OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public string OrganizationBin { get; set; }

        public string OrganizationAddress { get; set; }

        public int? BranchId { get; set; }

        public string BranchName { get; set; }

        public string BranchAddress { get; set; }

        public long ProductionReceiveId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int? BrandId { get; set; }

        public string BrandName { get; set; }

        public string Hscode { get; set; }

        public string ProductCode { get; set; }

        public string ModelNo { get; set; }

        public string Variant { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string ProductWeight { get; set; }

        public string Specification { get; set; }

        public int? ProductCategoryId { get; set; }

        public string ProductCategoryName { get; set; }

        public int ProductGroupId { get; set; }

        public string ProductGroupName { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductTypeName { get; set; }

        public decimal ReceiveQuantity { get; set; }

        public int MeasurementUnitId { get; set; }

        public string MeasurementUnitName { get; set; }

        public bool IsContractual { get; set; }

        public string ProductionType { get; set; }

		public string BatchNo { get; set; }

		public DateTime ReceiveTime { get; set; }

		public decimal MaterialCost { get; set; }
	}
}
