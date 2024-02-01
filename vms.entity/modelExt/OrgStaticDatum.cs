using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace vms.entity.models;

[ModelMetadataType(typeof(OrgStaticDatumMetadata))]
public partial class OrgStaticDatum : VmsBaseModel
{
        
}

public class OrgStaticDatumMetadata
{


	[Required]
	public int DataKey { get; set; }
	[Required]
	public string ReferenceKey { get; set; }
}