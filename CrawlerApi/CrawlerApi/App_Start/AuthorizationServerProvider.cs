using CrawlerApi.Models;
using IdentityConfig;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace CrawlerApi.App_Start
{
    public class AuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        // xác thực thằng client có phải là bạn mình ko
        public override Task ValidateClientAuthentication(
        OAuthValidateClientAuthenticationContext context)
        {
            //string clientId;
            //string clientSecret;
            //Debug.WriteLine("123");
            ////if (context.TryGetBasicCredentials(out clientId, out clientSecret))
            //if (true)
            //{

            //    Debug.WriteLine("1234");
            //    // validate the client Id and secret against database or from configuration file.
            //    context.Validated("1");
            //}
            //else
            //{
            //    Debug.WriteLine("1235");
            //    context.SetError("invalid_client", "Client credentials could not be retrieved from the Authorization header");
            //    context.Rejected();
            //}
            //only have one client so don need to validate client
            context.Validated();
            return Task.CompletedTask;
        }

        public override async Task GrantResourceOwnerCredentials(
        OAuthGrantResourceOwnerCredentialsContext context)
        {
            //config to enable cors at localhost domain
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            MyUserManager userManager = context.OwinContext.GetUserManager<IdentityConfig.MyUserManager>();
            AppUser user;
            try
            {
                Debug.WriteLine(context.UserName);
                Debug.WriteLine(context.Password);
                user = await userManager.FindAsync(context.UserName, context.Password);
            }
            catch
            {
                // Could not retrieve the user due to error.
                context.SetError("server_error");
                context.Rejected();
                return;
            }
            if (user != null)
            {
                Debug.WriteLine("Okie");
                ClaimsIdentity identity = await userManager.CreateIdentityAsync(
                user,
                DefaultAuthenticationTypes.ExternalBearer);
                context.Validated(identity);
            }
            else
            {
                Debug.WriteLine("Not okie");
                context.SetError("invalid_grant", "Invalid User Id or password'");
                context.Rejected();
            }
        }
    }
}