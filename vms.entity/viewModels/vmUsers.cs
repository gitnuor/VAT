namespace vms.entity.viewModels;

public class vmUsers
{
    public string UserName { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
    public string UserCode { get; set; }
    public string Code { get; set; }
    public int? uid { get; set; }
}