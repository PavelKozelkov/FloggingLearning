﻿using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Flogging.Web.Attributes;
using System.Web.Http.ExceptionHandling;
using Flogging.Web.Services;
using System.Web.Http.Cors;

namespace ToDoWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("https://samplemvc.knowyourtoolset.com,http://localhost:4200", "*", "*");            
            config.EnableCors(cors);

            // Web API configuration and services
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new AuthorizeAttribute());

            config.Filters.Add(new ApiLoggerAttribute("ToDos"));

            config.Services.Replace(typeof(IExceptionHandler), new CustomApiExceptionHandler());
            config.Services.Add(typeof(IExceptionLogger), new CustomApiExceptionLogger("ToDos"));            

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
