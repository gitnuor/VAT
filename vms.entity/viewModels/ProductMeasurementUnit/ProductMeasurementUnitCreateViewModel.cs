using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.entity.viewModels.ProductMeasurementUnit
{
	public class ProductMeasurementUnitCreateViewModel
	{
        public ProductMeasurementUnitCreateViewModel()
        {
			ProductList = new List<CustomSelectListItem>();
			MeasurementUnitList = new List<CustomSelectListItem>();
		}

        public int ProductMeasurementUnitId { get; set; }

		public IEnumerable<CustomSelectListItem> ProductList { get; set; }
		public IEnumerable<CustomSelectListItem> MeasurementUnitList { get; set; }


        [DisplayName("Product")]
		[Required]

		public int ProductId { get; set; }

		public int OrganizationId { get; set; }

		[DisplayName("Unit")]
		[Required]

		public int MeasurementUnitId { get; set; }

		[DisplayName("Conversion Ratio")]
		[Required]

		public decimal ConversionRatio { get; set; }

		public bool IsActive { get; set; }

		public int? CreatedBy { get; set; }

		public DateTime? CreatedTime { get; set; }

		public int? ModifiedBy { get; set; }

		public DateTime? ModifiedTime { get; set; }

		public virtual MeasurementUnit MeasurementUnit { get; set; }

		public virtual Product Product { get; set; }
	}
}
