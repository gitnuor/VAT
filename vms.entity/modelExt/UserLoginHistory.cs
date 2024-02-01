namespace vms.entity.models;

public partial class UserLoginHistory : VmsBaseModel
{
	public string LoginStatus => IsLoginAttemptSuccess ? "Yes" : "No";
}