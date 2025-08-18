using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VCheck.Modules.Fleet.Data;
using VCheck.SharedKernel;

namespace VCheck.Modules.Fleet
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFleetModule<TBuilder>(this TBuilder builder)
            where TBuilder : IHostApplicationBuilder
        {
            var services = builder.Services;            

            builder.AddFleetDbContext();

            services.AddScoped<IFleetModule, FleetModule>();
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }

        public static IServiceCollection AddFleetDbContext<TBuilder>(this TBuilder builder)
           where TBuilder : IHostApplicationBuilder
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            builder.AddSqlServerDbContext<FleetDbContext>("VCheckDb",
                configureDbContextOptions: opts =>
                {
                    opts.UseSqlServer(sql =>
                    {
                        sql.MigrationsHistoryTable("__EFMigrationsHistory", FleetDbContext.schema);
                    });

                    if (builder.Environment.IsDevelopment())
                    {
                        opts.EnableDetailedErrors();

                        // se quiser executar um seed/ensure-created em dev:
                        opts.LogTo(Console.WriteLine);
                    }
                });

            return services;
        }
    }
}
