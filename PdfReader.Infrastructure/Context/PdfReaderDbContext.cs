using MassTransit;
using Microsoft.EntityFrameworkCore;
using PdfReader.Core.Models;

namespace PdfReader.Infrastructure.Context;

public class PdfReaderDbContext: DbContext
{
    public PdfReaderDbContext(DbContextOptions<PdfReaderDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }
    public DbSet<PdfDocumentEntity> Documents { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}