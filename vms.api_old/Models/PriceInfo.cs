using System;
using System.Collections.Generic;
using vms.entity.models;

namespace vms.api.Models
{
    public class PriceInfo
    {
        public int PriceSetupId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string ProductName  { get; set; }
        public int ProductId { get; set; }
        public int MeasurementUnitId { get; set; }
        public string MeasurementUnitName { get; set; }
        public decimal? BaseTp { get; set; }
        public decimal? Mrp { get; set; }
        public decimal PurchaseUnitPrice { get; set; }
        public decimal SalesUnitPrice { get; set; }
        public bool? IsMushakSubmitted { get; set; }
        public int? SubmissionSl { get; set; }
        public DateTime? MushakSubmissionDate { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; }
        public string ReferenceKey { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public  List<PriceSetupProductCost> PriceSetupProductCosts { get; set; }
      
    }
}
