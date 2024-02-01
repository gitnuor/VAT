using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vms.entity.viewModels.ProductMeasurementUnit
{
	public class ProductMeasurementUnitIndexViewModel
	{
		public int ProductMeasurementUnitId { get; set; }

		public string EncryptedId { get; set; }

		public string ProductName { get; set; }

		public int OrganizationId { get; set; }

		public string MeasurementUnitName { get; set; }

		public decimal ConversionRatio { get; set; }

		public bool IsActive { get; set; }

		public int? CreatedBy { get; set; }

		public DateTime? CreatedTime { get; set; }

		public int? ModifiedBy { get; set; }

		public DateTime? ModifiedTime { get; set; }
	}
}
