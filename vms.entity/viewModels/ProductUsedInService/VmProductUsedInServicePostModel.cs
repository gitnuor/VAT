using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.entity.viewModels.ProductUsedInService
{
    public class VmProductUsedInServicePostModel
    {
        public VmProductUsedInServicePostModel()
        {
            OrgBranchList = new List<CustomSelectListItem>();
            ByProductList = new List<SelectListItem>();
            MeasurementUnitList = new List<SelectListItem>();
            CustomerList = new List<SelectListItem>();
        }

        public int ProductUsedInServiceId { get; set; }
        public IEnumerable<CustomSelectListItem> OrgBranchList { get; set; }
        public IEnumerable<SelectListItem> ByProductList { get; set; }
        public IEnumerable<SelectListItem> MeasurementUnitList { get; set; }
        public IEnumerable<SelectListItem> CustomerList { get; set; }
        public int OrganizationId { get; set; }
        [DisplayName("Branch")]
        [Required]
        public int OrgBranchId { get; set; }
        [DisplayName("Product")]
        [Required]
        public int ProductId { get; set; }
        [DisplayName("Customer")]
        [Required]
        public int CustomerId { get; set; }
        public decimal Quantity { get; set; }
        public decimal CurrentStock { get; set; }
        //[DisplayName("Conversion Ratio")]
        //public decimal ConversionRatio { get; set; }
        public int MeasurementUnitId { get; set; }
        public bool IsActive { get; set; }
        public string ReferenceKey { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime ModifiedTime { get; set; }
        public long? ApiTransactionId { get; set; }
        public long? ExcelDataUploadId { get; set; }
        public string EncryptedId { get; set; }
        public virtual MeasurementUnit MeasurementUnit { get; set; }
        public virtual OrgBranch OrgBranch { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
