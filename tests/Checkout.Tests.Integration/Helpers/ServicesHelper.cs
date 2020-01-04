using System;
using System.Collections.Generic;
using Checkout.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Tests.Integration.Helpers
{
    public class ServicesHelper
    {
        public IServiceCollection ServiceCollection { get; set; }

        public ServicesHelper()
        {
            ServiceCollection = new ServiceCollection();

            Startup startUp = new Startup(new ConfigurationRoot(new List<IConfigurationProvider>()));
            startUp.ConfigureServices(ServiceCollection);
        }
    }
}
