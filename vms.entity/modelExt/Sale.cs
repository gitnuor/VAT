
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using vms.entity.attr;

namespace vms.entity.models;

[ModelMetadataType(typeof(SaleMetadata))]
public partial class Sale : VmsBaseModel
{

	[NotMapped]
	public string CustomerReferenceId { get; set; }
	[NotMapped]
	public string VDS => IsVatDeductedInSource ? "Yes" : "No";
	[NotMapped]
	public string SalesStatus
	{
		get
		{
			if (IsApproved != true && IsRejected != true)
				return "Pending";
			if (IsApproved == true)
				return "Approved";
			if (IsRejected == true)
				return "Rejected";
			return "Complete";
		}
	}

	[NotMapped]
	public string MushakGenerateStatus => IsTaxInvoicePrined ? "Yes" : "No";
}
public class SaleMetadata
{
	[DisplayFormat(DataFormatString = "{0:d}")]
	public DateTime SalesDate { get; set; }
	[Display(Name = "Expected")]
	[ReportAttribute(Display = true)]
	[DisplayFormat(DataFormatString = "{0:d}")]
	public DateTime? ExpectedDeleveryDate { get; set; }
	[DisplayFormat(DataFormatString = "{0:d}")]
	public DateTime? DeliveryDate { get; set; }

	[Display(Name = "Expt. Del. Date")]
	public string ExpectedDeliveryDate { get; set; }

	[Display(Name = "Org.")]
	public string Organization { get; set; }


	[Display(Name = "Cus.Name")]
	public string CustomerName { get; set; }
	[Display(Name = "Cus.Add.")]
	public string CustomerAddress { get; set; }
	[Display(Name = "Is VDS")]
	public string ISVDS { get; set; }

	[Display(Name = "Inv.No.")]
	public string InvoiceNo { get; set; }

	//[Display(Name = "Sales Date.")]
	//public string SalesDate { get; set; }

	//[Display(Name = "Delivery Date.")]
	//public string DeliveryDate { get; set; }

        
	[ReportAttribute(NavigationTable = true)]
	public virtual Customer Customer { get; set; }
}