using System.Web.Http;
using Ninject;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using Updater.WebApi.Providers;

namespace Updater.WebApi
{
    public sealed class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();
            configuration.DependencyResolver = new OwinNinjectDependencyResolver(GetKernel());

            appBuilder.UseWebApi(configuration);
        }

        private StandardKernel GetKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ISecurityProvider>().To<FakeSecurityProvider>();
            kernel.Bind<ISourceProvider>().To<FakeSourceProvider>();
            return kernel;
        }
    }
}