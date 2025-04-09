using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FitnessTracker.Web.Attributes
{
    public class VirtualDomAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;
            
            // Check if this is an AJAX/VDOM request
            bool isVDomRequest = request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                                 request.Query.ContainsKey("vdom");

            if (isVDomRequest && context.Controller is Controller controller)
            {
                // Cast to Controller type to access ViewData
                controller.ViewData["IsVirtualDomRequest"] = true;
            }
            
            base.OnActionExecuted(context);
        }
    }
}