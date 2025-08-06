using ManiFest.Subscriber.Interfaces;
using ManiFest.Subscriber.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Add RabbitMQ services
        services.AddSingleton<IEmailSenderService, EmailSenderService>();
        services.AddHostedService<BackgroundWorkerService>();
    });

var host = builder.Build();
await host.RunAsync(); 