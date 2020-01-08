using System.IO;
using System.Reflection;
using Checkout.Repository;
using Checkout.Repository.Helpers;
using Checkout.Repository.LiteDb;
using Checkout.Services;
using Checkout.Services.Banks;
using Checkout.Web.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Checkout.Web.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;


namespace Checkout.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterDependencies(services);

            services.AddControllers();

            services.AddMvc();
            services.AddSwaggerExamplesFromAssemblyOf<PaymentGatewayController>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Gateway API Service v1", Version = "v1"});
                options.ExampleFilters();

                

                try
                {
                    var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                        "Checkout.Web.xml");

                    if (File.Exists(filePath))
                    {
                        options.IncludeXmlComments(filePath);
                    }
                }
                catch
                {
                    // Ignore if no XML file - means no comments for the fields.
                }
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            services.AddTransient<ISensitiveDataObfuscator, SensitiveDataObfuscator>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IBankFactory, BankFactory>();
            services.AddTransient<IPaymentOrchestrator, PaymentOrchestrator>();
            services.AddTransient<ILiteDatabaseWrapper, LiteDatabaseWrapper>();
            services.AddTransient<ICorrelationIdProvider, CorrelationIdProvider>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Custom middleware to handle all errors on middleware level
            app.UseErrorHandlingMiddleware();
            
            // Custom middleware to register existing correlation to context trace identifier
            app.UseCorrelationIdMiddleware();

            // Custom middleware to track start and end of a request
            app.UseRequestTimeTrackingMiddleware();


            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
