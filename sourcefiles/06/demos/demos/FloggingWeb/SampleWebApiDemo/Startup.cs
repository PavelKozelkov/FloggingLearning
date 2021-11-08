using IdentityServer3.AccessTokenValidation;
using Owin;

namespace SampleWebApiDemo
{
    public partial class Startup
    {        
        // Microsoft.Owin.Host.SystemWeb
        // Swashbuckle
        // Microsoft.AspNet.WebApi.Owin
        // IdentityServer3.AccessTokenValidation
        // Serilog.Sinks.File
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(
                new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = "https://demo.identityserver.io",
                    RequiredScopes = new[] { "api" }
                });
        }        
    }
}