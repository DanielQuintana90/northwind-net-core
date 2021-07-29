using System;
using AutoMapper.Configuration;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Northwind.Api;
using Northwind.Api.Repository;
using Northwind.Api.Repository.MySql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddNorthwindDependencies(this IServiceCollection services)
        {
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddAutoMapper(typeof(Startup));

            services.AddProblemDetails(opt => 
            {
                opt.IncludeExceptionDetails = (ctx, ex) => false;
                opt.ShouldLogUnhandledException = (ContextBoundObject, ex, details) => true;
                opt.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            });

            return services;
        }

        public static IServiceCollection AddHostDependencies(this IServiceCollection services, string connectionString)
        {
            services.AddControllers();
            
            services.AddDbContext<NorthwindDbContext>(opt => 
                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Northwind.Api", Version = "v1" });
                c.EnableAnnotations();                
                c.CustomSchemaIds(type => type.Name);
                //c.DocumentFilter<RemoveDefinitionDocumentFilter>();
            });    

            return services;
        }
    }
}
