using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.PurchaseImport;

public class PurchaseImportDetailPostWithPurchaseDto
{
	[DisplayName("Purchase Detail")]
	[Required]
	public string PurchaseDetailId { get; set; }

	[DisplayName("Product")]
	[Required(ErrorMessage = "Product is Required")]
	public string ProductId { get; set; }

	[DisplayName("Description")]
	[MaxLength(500)]
	public string ProductDescription { get; set; }

	[DisplayName("HS Code")]
	[MaxLength(50)]
	public string HSCode { get; set; }

	[DisplayName("Product Code")]
	[MaxLength(50)]
	public string ProductCode { get; set; }

	[DisplayName("Part No.")]
	[MaxLength(50)]
	public string PartNo { get; set; }

	[DisplayName("SKU")] [MaxLength(50)] public string SKUNo { get; set; }

	[DisplayName("SKU ID")]
	[MaxLength(50)]
	public string SKUId { get; set; }

	[DisplayName("Goods ID")]
	[MaxLength(50)]
	public string GoodsId { get; set; }
	[DisplayName("Measurement Unit")] public string MeasurementUnitName { get; set; }

	[DisplayName("Quantity")]
	[Required(ErrorMessage = "Quantity is Required")]
	[Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
	[RegularExpression(@"^\d+(\.\d{1,8})?$",
		ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
	public decimal Quantity { get; set; }

	[DisplayName("Assessable value")]
	[Required(ErrorMessage = "{0} is Required")]
	[Range(0.0001, 999999999999, ErrorMessage = "{1} to {2} value is allowed.")]
	[RegularExpression(@"^\d+(\.\d{1,8})?$",
		ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
	public decimal AssessableValue { get; set; }

	[DisplayName("CD (%)")]
	[Required(ErrorMessage = "{0} is Required")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	[RegularExpression(@"^\d+(\.\d{1,2})?$",
		ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
	public decimal CustomDutyPercent { get; set; }

	[DisplayName("ID (%)")]
	[Required(ErrorMessage = "{0} is Required")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	[RegularExpression(@"^\d+(\.\d{1,2})?$",
		ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
	public decimal ImportDutyPercent { get; set; }

	[DisplayName("RD (%)")]
	[Required(ErrorMessage = "{0} is Required")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	[RegularExpression(@"^\d+(\.\d{1,2})?$",
		ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
	public decimal RegulatoryDutyPercent { get; set; }

	[DisplayName("SD (%)")]
	[Required(ErrorMessage = "{0} is Required")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	[RegularExpression(@"^\d+(\.\d{1,2})?$",
		ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
	public decimal SupplementaryDutyPercent { get; set; }

	[DisplayName("VAT Type")]
	[Required(ErrorMessage = "{0} is Required")]
	public string ProductVatTypeId { get; set; }

	[DisplayName("VAT(%)")]
	[RegularExpression(@"^\d+(\.\d{1,2})?$",
		ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
	[Required(ErrorMessage = "{0} is Required")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	public decimal ProductVatPercent { get; set; }

	[DisplayName("AT (%)")]
	[Required(ErrorMessage = "{0} is Required")]
	[RegularExpression(@"^\d+(\.\d{1,2})?$",
		ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	public decimal AdvanceTaxPercent { get; set; }

	[DisplayName("AIT (%)")]
	[Required(ErrorMessage = "{0} is Required")]
	[RegularExpression(@"^\d+(\.\d{1,2})?$",
		ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	public decimal AdvanceIncomeTaxPercent { get; set; }
}