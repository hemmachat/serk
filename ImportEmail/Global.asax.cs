using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using ImportEmail.Interfaces;

namespace ImportEmail
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IContainer Container { get; set; }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var builder = new ContainerBuilder();
            builder.RegisterType<XmlUility>().As<IXmlUtility>();
            Container = builder.Build();
        }
    }
}
