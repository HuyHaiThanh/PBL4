using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace PBL4.Utilities
{
    public static class HttpClientFactoryUtility
    {
        public static IHttpClientFactory CreateFactory()
        {
            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            services.AddHttpClient();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IHttpClientFactory>();
        }
    }
}
