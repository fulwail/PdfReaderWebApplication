using MassTransit;
using Microsoft.EntityFrameworkCore;
using PdfReader.Core.Extensions;
using PdfReader.Infrastructure.Context;
using PdfReader.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddMapper();
builder.Services.AddServices(builder.Configuration);
var configurationRabbitMq = builder.Configuration.GetSection("RabbitMQ");
       
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(configurationRabbitMq["Host"], "/",h =>
        {
            h.Username(configurationRabbitMq["UserName"]);
            h.Password(configurationRabbitMq["Password"]);
        });
        
        cfg.ConfigureEndpoints(context);
    });
    x.AddEntityFrameworkOutbox<PdfReaderDbContext>(o =>
    {
        o.UsePostgres();
        o.UseBusOutbox();
        o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
    });
});






var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PdfReaderDbContext>();
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();