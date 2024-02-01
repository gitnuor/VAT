using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.entity.viewModels.ByProductReceive
{
    public class VmByProductReceivePostModel
    {
        public VmByProductReceivePostModel()
        {
            OrgBranchList = new List<CustomSelectListItem>();
            ByProductList = new List<CustomSelectListItem>();
            MeasurementUnitList = new List<CustomSelectListItem>();
        }

        public int ByProductReceiveId { get; set; }

        public IEnumerable<CustomSelectListItem> OrgBranchList { get; set; }
        public IEnumerable<CustomSelectListItem> ByProductList { get; set; }
        public IEnumerable<CustomSelectListItem> MeasurementUnitList { get; set; }

        public int OrganizationId { get; set; }

        [DisplayName("Branch")]
        [Required]

        public int OrgBranchId { get; set; }

        [DisplayName("Product")]
        [Required]

        public int ProductId { get; set; }

        [DisplayName("Product Description")]

        public string ProductDescription { get; set; }

        public decimal Quantity { get; set; }
        [DisplayName("Unit Price")]

        public decimal UnitPrice { get; set; }

        public int MeasurementUnitId { get; set; }
        [DisplayName("Receive Date")]
        public DateTime ReceiveDate { get; set; }

        public string Remarks { get; set; }

        public string ReferenceKey { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

        public string EncryptedId { get; set; }

        public virtual MeasurementUnit MeasurementUnit { get; set; }

        public virtual OrgBranch OrgBranch { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual Product Product { get; set; }
        public string ProductName { get; set; }
    }
}
