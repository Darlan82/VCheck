using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using VCheck.Modules.Fleet.Data;
using VCheck.SharedKernel;
using Microsoft.Extensions.Hosting;

namespace VCheck.Modules.Fleet
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFleetModule<TBuilder>(this TBuilder builder)
            where TBuilder : IHostApplicationBuilder
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            builder.AddSqlServerDbContext<FleetDbContext>("VCheckDb",
                configureDbContextOptions: opts =>
                {
                    if (builder.Environment.IsDevelopment())
                    {
                        opts.EnableDetailedErrors();

                        // se quiser executar um seed/ensure-created em dev:
                        opts.LogTo(Console.WriteLine);
                    }
                });

            services.AddScoped<IFleetModule, FleetModule>();
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);            

            return services;
        }
    }
}
