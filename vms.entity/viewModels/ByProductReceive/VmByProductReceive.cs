using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vms.entity.viewModels.ByProductReceive
{
    public class VmByProductReceive
    {
		public string EncryptedId { get; set; }
		public int ByProductReceiveId { get; set; }

        //public int OrganizationId { get; set; }

        //public int OrgBranchId { get; set; }
        public string BranchName { get; set; }

		public string ProductName { get; set; }

		//public int ProductId { get; set; }

		//[DisplayName("Product Description")]

		public string ProductDescription { get; set; }

        public decimal Quantity { get; set; }
        //[DisplayName("Unit Price")]

        public decimal UnitPrice { get; set; }

        //public int MeasurementUnitId { get; set; }
		public string MeasurementUnitName { get; set; }
		//[DisplayName("Receive Date")]
        public DateTime ReceiveDate { get; set; }

        public string Remarks { get; set; }

        public string ReferenceKey { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

       

        //public virtual MeasurementUnit MeasurementUnit { get; set; }

        //public virtual OrgBranch OrgBranch { get; set; }

        //public virtual Organization Organization { get; set; }

        //public virtual Product Product { get; set; }
    



        //public virtual ICollection<ProductTransactionBook> ProductTransactionBooks { get; set; } = new List<ProductTransactionBook>();
    }
}
