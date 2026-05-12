using MassTransit;
using PdfReader.Infrastructure.Extensions;
using PdfReader.Infrastructure.Services;
using PdfReader.Worker;
using PdfReader.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddServices(builder.Configuration);
var configurationRabbitMq = builder.Configuration.GetSection("RabbitMQ");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PdfUploadedConsumer>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(configurationRabbitMq["Host"],"/", h =>
        {
            h.Username(configurationRabbitMq["UserName"]);
            h.Password(configurationRabbitMq["Password"]);
        });
        
        cfg.ReceiveEndpoint("pdf-processing-queue", e =>
        {
            e.ConfigureConsumer<PdfUploadedConsumer>(context);
            
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
            
            e.PrefetchCount = 5;
            e.ConcurrentMessageLimit = 5;
        });
    });
});
var host = builder.Build();
host.Run();