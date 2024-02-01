using Microsoft.AspNetCore.Mvc;

namespace vms.entity.models;

[ModelMetadataType(typeof(IntegratedApplicationMetadata))]
public partial class IntegratedApplication : VmsBaseModel
{
}
public class IntegratedApplicationMetadata
{
}