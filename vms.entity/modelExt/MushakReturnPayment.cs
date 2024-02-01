using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(MushakReturnPaymentMetadata))]
public partial class MushakReturnPayment : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> typeList;
	[NotMapped]
	public string MonthName { get; set; }

}

public class MushakReturnPaymentMetadata
{
	[DisplayName("Treasury Challan No.")]
	[StringLength(40,ErrorMessage = "Treasury Challan No. can not have more than 40 characters")]
	public string TreasuryChallanNo { get; set; }
	[DisplayName("Subimission Date")]
	public DateTime? SubimissionDate { get; set; }
}