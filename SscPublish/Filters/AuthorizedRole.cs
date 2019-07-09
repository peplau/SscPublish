using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Sitecore;

namespace SscPublish.Filters
{
    /// <summary>
    /// Custom filter to give authorization for users in a specific role
    /// </summary>
    public class AuthorizedRole : AuthorizationFilterAttribute
    {
        private readonly string _role;

        public AuthorizedRole(string role)
        {
            _role = role;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            var context = Context.User;
            if (context.IsAuthenticated && (context.IsAdministrator || context.IsInRole(_role)))
                return;
            actionContext.Response =
                actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized,
                    "Unauthorized Access; User is " + Context.User.LocalName);
        }
    } 
}