using Microsoft.OpenApi;
using OPAOWebService.Server.Business;
using OPAOWebService.Server.Business.Interfaces;
using OPAOWebService.Server.Data.Providers;
using OPAOWebService.Server.Data.Providers.Interfaces;
using OPAOWebService.Server.Data.Repositories;
using OPAOWebService.Server.Data.Repositories.Interfaces;

namespace OPAOWebService.Server.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            // Database Connection
            services.AddSingleton<IDatabaseConnection, DatabaseConnection>();

            // Business & Data Layers
            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<ITaxRepository, TaxRepository>();

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