using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SimplePrices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuration et services API Web
            config.EnableCors();
            // Itinéraires de l'API Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config
                .Formatters
                .JsonFormatter
                .SupportedMediaTypes
                .Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            config
                .Formatters
                .JsonFormatter
                .SerializerSettings
                .Formatting = Newtonsoft.Json.Formatting.Indented;


            //config.Routes.MapHttpRoute(
            //    name: "Other",
            //    routeTemplate: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Product", action = "Get", id = RouteParameter.Optional }
            //    );
        }
    }
}
