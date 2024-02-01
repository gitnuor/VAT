using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using vms.utility.StaticData;

namespace vms.Utility;

public class VmsAuthorizeAttribute : ActionFilterAttribute
{
    private readonly string _right;

    public VmsAuthorizeAttribute(string right)
    {
        _right = right;
    }
        

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var session = filterContext.HttpContext.Session.GetComplexData<entity.viewModels.VmUserSession>(ControllerStaticData.SESSION);
        if (!UserAuthorization.Check(_right, session.Rights))
        {

            filterContext.Result = new RedirectResult("~/");
            return;
        }

        base.OnActionExecuting(filterContext);
    }
        

}