using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;

namespace EventManagementSystem.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeToken : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext actionContext)
        {
            string token = actionContext.HttpContext.Session.GetString("token");
            string email = actionContext.HttpContext.Session.GetString("Email");

            //CookieHeaderValue cookie = _request.Headers.GetCookies("session-id").FirstOrDefault();
            //var tokenDetail = actionContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split("")[1];
            //var token = HttpContext.Session("token");
            var handler = new JwtSecurityTokenHandler();
            if (token != null)
            {
                var jsonToken = handler.ReadToken(token);
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                if (tokenS != null && email != null)
                {
                    actionContext.HttpContext.Response.Headers.Add("AuthorizeToken", token);
                    actionContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
                    actionContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");
                    return;
                }
                else
                {
                    actionContext.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                    return;
                }
            }
            else
            {
                actionContext.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }
        }

    }
}
