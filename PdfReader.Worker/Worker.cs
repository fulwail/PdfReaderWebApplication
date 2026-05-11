using Microsoft.EntityFrameworkCore;
using PdfReader.Infrastructure.Context;

namespace PdfReader.Worker;

public class Worker(ILogger<Worker> logger
    //,PdfReaderDbContext context
    ) : BackgroundService
{
 
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      //  await context.Database.MigrateAsync(stoppingToken);
        
        logger.LogInformation("PDF Reader Worker started");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}