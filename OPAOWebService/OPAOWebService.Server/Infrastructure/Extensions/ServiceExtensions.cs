using Microsoft.OpenApi;
using OPAOWebService.Server.Business;
using OPAOWebService.Server.Business.Interfaces;
using OPAOWebService.Server.Data.Providers;
using OPAOWebService.Server.Data.Providers.Interfaces;
using OPAOWebService.Server.Data.Repositories;
using OPAOWebService.Server.Data.Repositories.Interfaces;
using OPAOWebService.Server.Factories;
using OPAOWebService.Server.Factories.Interfaces;
using OPAOWebService.Server.Infrastructure.Security;
using OPAOWebService.Server.Infrastructure.Security.Interfaces;

namespace OPAOWebService.Server.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            // Data Protection Decryption
            services.AddSingleton<IConfigProtector, ConfigProtector>();

            // Database Connection
            services.AddSingleton<IDatabaseConnection, DatabaseConnection>();

            services.AddScoped<ITransactionClientFactory, TransactionClientFactory>();
            services.AddScoped<ITransactionGetRequestFactory, TransactionGetRequestFactory>();
            services.AddScoped<ITransactionBindingProvider,  IasWorldBindingProvider>();

            // Business & Data Layers
            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<ITaxRepository, TaxRepository>();

            services.AddScoped<ILogService, LogService>();


            return services;
        }

        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "OPAOWebService API",
                    Version = "v1.0.0",
                    Description = "A comprehensive web service for OPAO operations, providing endpoints for data management and integration.",
                    Contact = new OpenApiContact
                    {
                        Name = "Joseph Adogeri",
                        Email = "support@opao.com",
                        Url = new Uri("https://opao.example.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Apache 2.0 (License)",
                        Url = new Uri("http://apache.org")
                    }
                });
            });
        }
    }
}