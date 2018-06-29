using Autofac;
using Autofac.Integration.WebApi;
using ImportEmail.Controllers;
using ImportEmail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;

namespace ImportEmail.Test
{
    // https://github.com/imperugo/Spike/blob/master/imperugo.webapi.selfhost/imperugo.webapi.integrationTest/WebApiClassBase.cs
    public abstract class WebApiTestBase : IDisposable
    {
        private readonly string baseAddress;
        private HttpSelfHostConfiguration configuration;
        private HttpSelfHostServer server;
        private readonly Type controllerType;

        protected WebApiTestBase(Type controllerType)
            : this("localhost", 8080, controllerType)//
        { }

        protected WebApiTestBase(string host, int port, Type controllerType)
        {
            this.controllerType = controllerType;
            if (string.IsNullOrEmpty(host))
            {
                host = "localhost";
            }

            baseAddress = string.Format("http://{0}:{1}", host, port);
        }

        public virtual HttpSelfHostConfiguration Configuration
        {
            get
            {
                if (configuration == null)
                {
                    configuration = new HttpSelfHostConfiguration(baseAddress);
                    var builder = new ContainerBuilder();
                    builder.RegisterApiControllers(controllerType.Assembly);

                    // need to put all controller dependencies here 
                    builder.Register<IXmlUtility>(c => new XmlUtility()).SingleInstance();

                    var container = builder.Build();

                    var resolver = new AutofacWebApiDependencyResolver(container);
                    configuration.DependencyResolver = resolver;
                    configuration.Services.Replace(typeof(IAssembliesResolver), new TestAssemblyResolver(controllerType));
                    configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
                    configuration.Routes.MapHttpRoute(
                        name: "DefaultApi",
                        routeTemplate: "api/{controller}/{id}",
                        defaults: new { id = RouteParameter.Optional }
                    );
                }

                return configuration;
            }
        }

        public virtual HttpSelfHostServer Server
        {
            get { return server ?? (server = new HttpSelfHostServer(Configuration)); }
        }

        public string BaseAddress
        {
            get { return baseAddress; }
        }

        public void Start()
        {
            Server.OpenAsync().Wait();
        }

        public void Close()
        {
            Server.CloseAsync().Wait();
        }

        protected HttpResponseMessage CreateRequest(string url, HttpMethod method, string json)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseAddress + url);
            request.Method = method;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(json))
            {
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var client = new HttpClient(this.Server);

            return client.SendAsync(request).Result;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (configuration != null)
            {
                configuration.Dispose();
                configuration = null;
            }

            if (server != null)
            {
                server.Dispose();
                server = null;
            }
        }

        #endregion


        public class TestAssemblyResolver : IAssembliesResolver
        {
            private readonly Type controllerType;

            public TestAssemblyResolver(Type controllerType)
            {
                this.controllerType = controllerType;
            }

            public ICollection<Assembly> GetAssemblies()
            {
                List<Assembly> baseAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

                if (!baseAssemblies.Contains(controllerType.Assembly))
                    baseAssemblies.Add(controllerType.Assembly);

                return baseAssemblies;
            }
        }
    }
}