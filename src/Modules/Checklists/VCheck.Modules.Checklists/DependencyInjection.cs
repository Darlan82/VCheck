using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using VCheck.Modules.Checklists.Data;
using VCheck.SharedKernel;
using Microsoft.Extensions.Hosting;

namespace VCheck.Modules.Checklists
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChecklistsModule<TBuilder>(this TBuilder builder)
            where TBuilder : IHostApplicationBuilder
        {
            var services = builder.Services;

            builder.AddChecklistsDbContext();

            services.AddScoped<IChecklistsModule, ChecklistsModule>();
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }

        public static IServiceCollection AddChecklistsDbContext<TBuilder>(this TBuilder builder)
            where TBuilder : IHostApplicationBuilder
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            builder.AddSqlServerDbContext<ChecklistsDbContext>("VCheckDb",
                configureDbContextOptions: opts =>
                {
                    opts.UseSqlServer(sql =>
                    {
                        sql.MigrationsHistoryTable("__EFMigrationsHistory", ChecklistsDbContext.schema);
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
