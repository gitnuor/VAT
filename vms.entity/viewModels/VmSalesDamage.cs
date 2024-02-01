using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels;

public class VmSalesDamage
{
    //Main Master Page
    public int DamageTypeId { get; set; }
    [DisplayName("Voucher No")]
    [Required(ErrorMessage = "Voucher No is Required")]
    [MaxLength(30)]
    public string VoucherNo { get; set; }

    [DisplayName("Description")]
    [MaxLength(50)]
    [Required(ErrorMessage = "Description is Required")]
    public string Description { get; set; }
    [DisplayName("NBR Verifier Name")]
    [MaxLength(30)]
    public string NbrverifierName { get; set; }
    [DisplayName("NBR Verifier Designation")]
    [MaxLength(30)]
    public string NbrverifierDesignation { get; set; }

    public string ReferenceKey { get; set; }

    public int SalesId { get; set; }
    //Details

    public int? SalesDetailId { get; set; }
    public int MeasurementUnitId { get; set; }
    [DisplayName("Poduct")]
    [Required(ErrorMessage ="Product is Required")]
    public int ProductId { get; set; }
    [DisplayName("Damage Qty")]
    [Required(ErrorMessage = "Damage Qty is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal DamageQty { get; set; }
    [DisplayName("Usable Qty")]
    [Required(ErrorMessage = "Usable Qty is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal UsableQty { get; set; }
    [DisplayName("Usable Percent")]
    public decimal UsablePercent { get; set; }
    [DisplayName("New Unit Price")]
    [Required(ErrorMessage ="New Unit Price is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal SuggestedNewUnitPrice { get; set; }
    [DisplayName("Damage Description")]
    [Required(ErrorMessage ="Damage Description")]
    [MaxLength(50)]     
    public string DamageDescription { get; set; }

    public Damage Damage { get; set; }
    public List<DamageDetail> DamageDetails { get; set; }
}


public class VmSalesDamagePost
{
    public VmSalesDamagePost()
    {
        DamageDetailList = new List<VmSalesDamageDetail>();
    }

    public List<VmSalesDamageDetail> DamageDetailList { get; set; }
    //Main Master Page
    public int DamageTypeId { get; set; }
    public string VoucherNo { get; set; }
    public int SalesId { get; set; }
    public string Description { get; set; }
    public string NbrverifierName { get; set; }
    public string NbrverifierDesignation { get; set; }

    public string ReferenceKey { get; set; }

}

public class VmSalesDamageDetail
{
    public int? SalesDetailId { get; set; }
    public int MeasurementUnitId { get; set; }
    public int ProductId { get; set; }
    public decimal DamageQty { get; set; }
    public decimal UsableQty { get; set; }
    public decimal? UsablePercent => (UsableQty * 100) / DamageQty;
    public decimal SuggestedNewUnitPrice { get; set; }

    public string DamageDescription { get; set; }
}