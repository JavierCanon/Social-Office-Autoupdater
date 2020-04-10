using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Updater.WebApi.Providers;

namespace Updater.WebApi
{
    internal sealed class AuthorizationAttribute : AuthorizeAttribute
    {
        private const string AuthorizationHeader = "authorization";

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext != null
                && actionContext.Request != null
                && actionContext.Request.Headers.Contains(AuthorizationHeader)
                && actionContext.Response == null)
            {
                var securityProvider = actionContext.Request.GetDependencyScope().GetService(typeof (ISecurityProvider)) as ISecurityProvider;
                if (securityProvider != null)
                {
                    var token = actionContext.Request.Headers.GetValues(AuthorizationHeader).FirstOrDefault();
                    if (!String.IsNullOrWhiteSpace(token))
                    {
                        return securityProvider.IsValid(new Dictionary<string, string>
                        {
                            {AuthorizationHeader, token}
                        });
                    }
                }
            }

            return false;
        }
    }
}