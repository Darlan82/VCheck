using VCheck.Modules.Checklists.Data;
using Microsoft.EntityFrameworkCore;

namespace VCheck.Modules.Checklists.MigrationService;

public class Worker(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChecklistsDbContext>();

        await RunMigrationAsync(dbContext, cancellationToken);

        //Usar para dados padrão da base de dados
        //await SeedDataAsync(dbContext, cancellationToken);

        //var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
        //if (env.IsDevelopment())
        //{
        //    //Massa de dados para desenvolvimento
        //    await DbDevInitializer.InitializeAsync(db);
        //}

        hostApplicationLifetime.StopApplication();
    }

    private static async Task RunMigrationAsync(ChecklistsDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }


    //private static async Task SeedDataAsync(FleetDbContext dbContext, CancellationToken cancellationToken)
    //{
    //    var strategy = dbContext.Database.CreateExecutionStrategy();
    //    await strategy.ExecuteAsync(async () =>
    //    {
    //        // Seed the database
    //        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    //        await dbContext.Tickets.AddAsync(firstTicket, cancellationToken);
    //        await dbContext.SaveChangesAsync(cancellationToken);
    //        await transaction.CommitAsync(cancellationToken);
    //    });
    //}
}
