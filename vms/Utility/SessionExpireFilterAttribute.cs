using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using vms.utility.StaticData;

namespace vms.Utility;

public class SessionExpireFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var session = filterContext.HttpContext.Session.GetComplexData<entity.viewModels.VmUserSession>(ControllerStaticData.SESSION);
        if (session == null)
        {

            // check if a new session id was generated
            filterContext.Result = new RedirectResult("~/Authentication/Index");
            return;
        }

        base.OnActionExecuting(filterContext);
    }
}