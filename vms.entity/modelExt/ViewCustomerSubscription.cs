using Microsoft.AspNetCore.Mvc;

namespace vms.entity.models;

[ModelMetadataType(typeof(ViewCustomerSubscriptionMetadata))]
public partial class ViewCustomerSubscription : VmsBaseModel;

public class ViewCustomerSubscriptionMetadata;